using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ASI.Basecode.WebApp.Controllers
{
    public class TicketController : ControllerBase<TicketController>
    {
        private readonly ITicketService _ticketService;
        private readonly IResponseService _responseService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        public TicketController(ITicketService ticketService,
                                IResponseService responseService,
                                ICategoryService categoryService,
                                IUserService userService,
                                IHttpContextAccessor httpContextAccessor,
                                ILoggerFactory loggerFactory,
                                IConfiguration configuration,
                                IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _responseService = responseService;
            _ticketService = ticketService;
            _categoryService = categoryService;
            _userService = userService;
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
                    Assignee = ticket.Assignee,
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
        public IActionResult AddTicket(TicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newTicket = new Ticket
                    {
                        Subject = model.Subject,
                        RequesterEmail = model.RequesterEmail,
                        Category = model.Category,
                        Assignee = model.Assignee,
                        Priority = model.Priority,
                        Status = model.Status,
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now
                    };

                    _ticketService.AddTicket(newTicket);

                    TempData["SuccessMessage"] = "New ticket added.";
                    return RedirectToAction("Index");
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
                    Assignee = ticket.Assignee,
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
                        CategoryName = selectedCategory?.CategoryName,
                        RequesterEmail = ticket.RequesterEmail,
                        Assignee = ticket.Assignee,
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
                    CreatedTime = r.CreatedTime
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
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateTicket(int ticketId, string description, string status, int category, string priority, string assignee)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Model error: {modelError.ErrorMessage}");
                }

                return View();
            }

            var ticket = _ticketService.GetTicketById(ticketId);

            if (ticket == null)
            {
                Console.WriteLine($"Ticket with ID {ticketId} was not found.");
                return NotFound();
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                ticket.Category = category;
                ticket.Priority = priority;
                ticket.Assignee = assignee;
                ticket.Status = status;
                ticket.UpdatedTime = DateTime.Now;

                _ticketService.UpdateTicket(ticket);

                if (!string.IsNullOrEmpty(description))
                {
                    var newResponse = new Response
                    {
                        TicketId = ticketId,
                        Sender = userId ?? "Unknown",
                        Description = description,
                        CreatedTime = DateTime.Now
                    };

                    _responseService.AddResponse(newResponse);
                }

                TempData["SuccessMessage"] = "Ticket has been updated.";
                return RedirectToAction("View", new { ticketId = ticketId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating ticket: {ex.Message}");
                return StatusCode(500, "Internal server error");
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
                _responseService.DeleteResponse(id);
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
