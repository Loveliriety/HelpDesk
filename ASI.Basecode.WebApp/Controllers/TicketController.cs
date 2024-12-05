using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ASI.Basecode.Data.PathManager;

namespace ASI.Basecode.WebApp.Controllers 
{
    public class TicketController : ControllerBase<TicketController>
    {
        private readonly ITicketService _ticketService;
        private readonly IResponseService _responseService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        private readonly IAttachmentService _attachmentService;

        public TicketController(ITicketService ticketService,
                                IResponseService responseService,
                                ICategoryService categoryService,
                                IUserService userService,
                                ITeamService teamService,
                                IAttachmentService attachmentService,
                                IHttpContextAccessor httpContextAccessor,
                                ILoggerFactory loggerFactory,
                                IConfiguration configuration,
                                IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _responseService = responseService;
            _ticketService = ticketService;
            _categoryService = categoryService;
            _userService = userService;
            _teamService = teamService;
            _attachmentService = attachmentService;
        }

        // Index
        public IActionResult Index(int? categoryId)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole == "User")
            {
                return RedirectToAction("Index", "Users");
            }

            var (success, tickets) = _ticketService.GetAllTickets();
            var categories = _categoryService.GetAllCategories();
            var users = _userService.GetUsers();
            var teams = _teamService.GetAllTeams();

            if (!success)
            {
                return BadRequest();
            }

            var categoryCounts = categories.ToDictionary(
                category => category.CategoryId,
                category => tickets.Count(ticket => ticket.Category == category.CategoryId)
            );

            if (categoryId.HasValue)
            {
                tickets = tickets.Where(ticket => ticket.Category == categoryId.Value).ToList();
                ViewData["SelectedCategory"] = categoryId;
            }

            ViewData["CategoryCounts"] = categoryCounts;

            var model = new TicketListViewModel
            {
                Tickets = tickets.Select(ticket => new TicketPageViewModel
                {
                    TicketId = ticket.TicketId,
                    Status = ticket.Status,
                    Subject = ticket.Subject,
                    Category = ticket.Category,
                    RequesterEmail = ticket.RequesterEmail,
                    Assignee = _teamService.GetTeamNameById(ticket.Assignee),
                    Priority = ticket.Priority,
                    CreatedTime = ticket.CreatedTime,
                    UpdatedTime = ticket.UpdatedTime
                }).ToList(),

                Categories = categories.Select(category => new CategoryViewModel
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                }).ToList(),

                Users = users.Select(user => new UserViewModel
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    TeamId = user.TeamId,
                }).ToList(),

                Teams = teams.Select(team => new TeamViewModel
                {
                    TeamId = team.TeamId,
                    TeamName = team.TeamName

                }).ToList()
            };

            return View(model);
        }

        // GET: NewTicket
        public IActionResult NewTicket()
        {
            var categories = _categoryService.GetAllCategories();
            var users = _userService.GetUsers();

            return View(new { Categories = categories, Users = users });
        }

        // POST: NewTicket
        [HttpPost]
        public IActionResult AddTicket(Ticket model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int ticketId = _ticketService.AddTicket(model);

                    TempData["SuccessMessage"] = "New ticket added.";
                    return RedirectToAction("View", new { ticketId = ticketId });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    TempData["ErrorMessage"] = "An error occurred while creating the ticket.";
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                TempData["ErrorMessage"] = "Validation failed. Please check your inputs.";
            }

            return RedirectToAction("Index");
        }


        // MyTicket
        [HttpGet]
        public IActionResult MyTicket(string status = null)
        {
            var (success, tickets) = _ticketService.GetAllTickets();
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdString);

            if (!success)
            {
                return BadRequest("Failed to fetch tickets.");
            }

            var allowedStatuses = new[] { "Open", "In Progress", "Resolved", "Closed" };

            foreach (var ticket in tickets)
            {
                Console.WriteLine($"Ticket ID: {ticket.TicketId}, Status: {ticket.Status}, Assignee: {ticket.Assignee}");
            }

            tickets = tickets
                .Where(ticket => ticket.Assignee == userId && allowedStatuses.Contains(ticket.Status))
                .ToList();

            var statusCounts = allowedStatuses.ToDictionary(
                status => status,
                status => tickets.Count(ticket => ticket.Status == status)
            );

            foreach (var kvp in statusCounts)
            {
                Console.WriteLine($"Status: {kvp.Key}, Count: {kvp.Value}");
            }

            if (!string.IsNullOrEmpty(status) && allowedStatuses.Contains(status))
            {
                tickets = tickets.Where(ticket => ticket.Status == status).ToList();
                ViewData["SelectedStatus"] = status;
            }

            ViewData["StatusCounts"] = statusCounts;

            var model = new TicketListViewModel
            {
                Tickets = tickets.Select(ticket => new TicketPageViewModel
                {
                    TicketId = ticket.TicketId,
                    Status = ticket.Status,
                    Subject = ticket.Subject,
                    Category = ticket.Category,
                    RequesterEmail = ticket.RequesterEmail,
                    Assignee = _teamService.GetTeamNameById(ticket.Assignee),
                    Priority = ticket.Priority,
                    CreatedTime = ticket.CreatedTime,
                    UpdatedTime = ticket.UpdatedTime
                }).ToList()
            };

            return View(model);
        }

        // Ticket View
        [HttpGet]
        public IActionResult View(int ticketId)
        {
            var ticket = _ticketService.GetTicketById(ticketId);
            var categories = _categoryService.GetAllCategories();
            var users = _userService.GetUsers();
            var responses = _responseService.GetResponsesByTicketId(ticketId);
            var teams = _teamService.GetAllTeams();

            if (ticket == null)
            {
                return NotFound();
            }

            var selectedCategory = _categoryService.GetCategoryById(ticket.Category);

            var model = new TicketListViewModel
            {
                Tickets = new List<TicketPageViewModel>
                {
                    new TicketPageViewModel
                    {
                        TicketId = ticket.TicketId,
                        Status = ticket.Status,
                        Subject = ticket.Subject,
                        CategoryId = ticket.Category,
                        Description = ticket.Description,
                        CategoryName = selectedCategory?.CategoryName,
                        RequesterEmail = ticket.RequesterEmail,
                        Assignee = _teamService.GetTeamNameById(ticket.Assignee),
                        Priority = ticket.Priority,
                        CreatedTime = ticket.CreatedTime,
                        UpdatedTime = ticket.UpdatedTime,
                    }
                },

                Responses = responses.Select(r => new ResponseViewModel
                {
                    ResponseId = r.ResponseId,
                    TicketId = r.TicketId,
                    Sender = r.Sender,
                    Description = r.Description,
                    CreatedTime = r.CreatedTime,
                    Attachments = _attachmentService.GetAttachmentsByResponseId(r.ResponseId)
                        .Select(a => new AttachmentViewModel
                        {
                            AttachmentId = a.AttachmentId,
                            ResponseId = a.ResponseId,
                            FileData = a.File != null ? Convert.ToBase64String(a.File) : null 
                        }).ToList()
                }).ToList(),

                Categories = categories.Select(category => new CategoryViewModel
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName
                }).ToList(),

                Users = users.Select(user => new UserViewModel
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    TeamId = user.TeamId,
                }).ToList(),

                Teams = teams.Select(team => new TeamViewModel
                {
                    TeamId = team.TeamId,
                    TeamName = team.TeamName

                }).ToList()

            };

            return View(model);
        }

        [HttpPost]
        public Task<IActionResult> UpdateTicket(Ticket ticket, Response response, List<IFormFile> attachments)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid.");
                return Task.FromResult<IActionResult>(View());
            }

            var existingTicket = _ticketService.GetTicketById(ticket.TicketId);
            if (existingTicket == null)
            {
                Console.WriteLine($"Ticket with ID {ticket.TicketId} was not found.");
                return Task.FromResult<IActionResult>(NotFound());
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                existingTicket.Category = ticket.Category;
                existingTicket.Priority = ticket.Priority;
                existingTicket.Assignee = ticket.Assignee;
                existingTicket.Status = ticket.Status;
                existingTicket.UpdatedTime = DateTime.Now;

                _ticketService.UpdateTicket(existingTicket);

                if (!string.IsNullOrEmpty(response.Description))
                {
                    response.TicketId = ticket.TicketId;
                    response.Sender = userId ?? "Unknown";
                    response.CreatedTime = DateTime.Now;

                    var responseId = _responseService.AddResponse(response);

                    if (attachments != null && attachments.Count > 0)
                    {
                        foreach (var attachment in attachments.Take(5)) 
                        {
                            if (attachment.Length > 0)
                            {
                                var attachmentEntity = new Attachment
                                {
                                    ResponseId = responseId,
                                    File = GetFileData(attachment) 
                                };

                                _attachmentService.AddAttachment(attachmentEntity);
                            }
                        }
                    }
                }

                TempData["SuccessMessage"] = "Ticket has been updated.";
                return Task.FromResult<IActionResult>(RedirectToAction("View", new { ticketId = ticket.TicketId }));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating ticket: {ex.Message}");
                return Task.FromResult<IActionResult>(StatusCode(500, "Internal server error"));
            }
        }

        private byte[] GetFileData(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream); 
                return memoryStream.ToArray();
            }
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ticket ID.");
            }

            try
            {
                _ticketService.DeleteTicket(id);
                var responses = _responseService.GetResponsesByTicketId(id).ToList();
                foreach (var response in responses)
                {
                    _attachmentService.DeleteAttachmentsByResponseId(response.ResponseId);
                }
                _responseService.DeleteResponsesByTicketId(id);
                TempData["SuccessMessage"] = "Ticket has been deleted";

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting ticket: {ex.Message}");
            }
        }



    }
}
