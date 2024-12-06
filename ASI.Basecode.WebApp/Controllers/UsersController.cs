using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UsersController : ControllerBase<UsersController>
    {
        private readonly ICategoryService _categoryService;
        private readonly ITicketService _ticketService;
        private readonly IResponseService _responseService;
        private readonly IUserService _userService;
        private readonly IAttachmentService _attachmentService;
        public UsersController(ICategoryService categoryService,
                                ITicketService ticketService,
                                IResponseService responseService,
                                IUserService userService,
                                IAttachmentService attachmentService,
                                IHttpContextAccessor httpContextAccessor,
                                ILoggerFactory loggerFactory,
                                IConfiguration configuration,
                                IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _categoryService = categoryService;
            _ticketService = ticketService;
            _userService = userService;
            _responseService = responseService;
            _attachmentService = attachmentService;
        }

        public IActionResult Index(string status = null)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userEmail = HttpContext.Session.GetString("Email");

            if (userRole != "User")
            {
                return RedirectToAction("Index", "Home");
            }

            var (success, tickets) = _ticketService.GetAllTickets();
            if (!success)
            {
                return BadRequest("Failed to fetch tickets.");
            }

            var allowedStatuses = new[] { "Open", "In Progress", "Resolved", "Closed" };

            tickets = tickets
                .Where(ticket => ticket.RequesterEmail == userEmail && allowedStatuses.Contains(ticket.Status))
                .ToList();

            if (!string.IsNullOrEmpty(status) && allowedStatuses.Contains(status))
            {
                tickets = tickets.Where(ticket => ticket.Status == status).ToList();
                ViewData["SelectedStatus"] = status;
            }

            ViewData["StatusCounts"] = allowedStatuses.ToDictionary(
                stat => stat,
                stat => tickets.Count(ticket => ticket.Status == stat)
            );

            var model = new TicketListViewModel
            {
                Tickets = tickets.Select(ticket => new TicketPageViewModel
                {
                    TicketId = ticket.TicketId,
                    Status = ticket.Status,
                    Subject = ticket.Subject,
                    Description = ticket.Description,
                    Category = ticket.Category,
                    RequesterEmail = ticket.RequesterEmail,
                    Priority = ticket.Priority,
                    CreatedTime = ticket.CreatedTime,
                    UpdatedTime = ticket.UpdatedTime
                }).ToList()
            };

            return View(model);
        }



        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "User")
            {
                return RedirectToAction("Index", "Home");
            }
            var categories = _categoryService.GetAllCategories();

            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View();
        }

        [HttpPost]
        public IActionResult Create(string Subject, string Description, int CategoryId, string Priority)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "User")
            {
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrWhiteSpace(Subject) || string.IsNullOrWhiteSpace(Description) || CategoryId <= 0 || string.IsNullOrWhiteSpace(Priority))
            {
                TempData["ErrorMessage"] = "All fields are required. Please fill in the form completely.";
                return RedirectToAction("Create");
            }

            try
            {
                var email = HttpContext.Session.GetString("Email");

                var newTicket = new Ticket
                {
                    Subject = Subject,
                    Description = Description,
                    Category = CategoryId,
                    Priority = Priority,
                    RequesterEmail = email,
                    Status = "Open",
                    //Assignee = null, 
                };

                _ticketService.AddTicket(newTicket);

                TempData["SuccessMessage"] = "Your ticket has been submitted successfully.";
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating ticket: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while submitting your ticket. Please try again.";
                return RedirectToAction("Create");
            }
        }

        public IActionResult MyTickets(string status = null, int? selectedTicketId = null)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (userRole != "User")
            {
                return RedirectToAction("Index", "Home");
            }

            var userEmail = HttpContext.Session.GetString("Email");

            var (success, tickets) = _ticketService.GetAllTickets();
            if (!success)
            {
                return BadRequest("Failed to fetch tickets.");
            }

            var allowedStatuses = new[] { "Open", "In Progress", "Resolved", "Closed" };

            tickets = tickets
                .Where(ticket => ticket.RequesterEmail == userEmail && allowedStatuses.Contains(ticket.Status))
                .ToList();

            if (!string.IsNullOrEmpty(status) && allowedStatuses.Contains(status))
            {
                tickets = tickets.Where(ticket => ticket.Status == status).ToList();
                ViewData["SelectedStatus"] = status;
            }

            Ticket selectedTicket = null;
            List<Response> responses = new List<Response>();
            if (selectedTicketId.HasValue)
            {
                selectedTicket = tickets.FirstOrDefault(ticket => ticket.TicketId == selectedTicketId.Value);

                responses = _responseService.GetResponsesByTicketId(selectedTicketId.Value);
                if (responses == null || !responses.Any())
                {
                    Console.WriteLine("No responses found for ticket ID: " + selectedTicketId.Value);
                }

                Console.WriteLine("Selected Ticket ID: " + selectedTicketId);
            }

            ViewData["StatusCounts"] = allowedStatuses.ToDictionary(
                stat => stat,
                stat => tickets.Count(ticket => ticket.Status == stat)
            );

            var model = new TicketListViewModel
            {
                Tickets = tickets.Select(ticket => new TicketPageViewModel
                {
                    TicketId = ticket.TicketId,
                    Status = ticket.Status,
                    Subject = ticket.Subject,
                    Category = ticket.Category,
                    RequesterEmail = ticket.RequesterEmail,
                    Priority = ticket.Priority,
                    CreatedTime = ticket.CreatedTime,
                    UpdatedTime = ticket.UpdatedTime
                }).ToList(),
                SelectedTicket = selectedTicket != null ? new TicketPageViewModel
                {
                    TicketId = selectedTicket.TicketId,
                    Status = selectedTicket.Status,
                    Subject = selectedTicket.Subject,
                    Category = selectedTicket.Category,
                    RequesterEmail = selectedTicket.RequesterEmail,
                    Priority = selectedTicket.Priority,
                    CreatedTime = selectedTicket.CreatedTime,
                    UpdatedTime = selectedTicket.UpdatedTime,
                    Description = selectedTicket.Description
                } : null,
                Responses = responses.Select(r => new ResponseViewModel
                {
                    ResponseId = r.ResponseId,
                    TicketId = r.TicketId,
                    Sender = r.Sender == userId ? "You" : "Customer Support",
                    Description = r.Description,
                    CreatedTime = r.CreatedTime,
                    Attachments = _attachmentService.GetAttachmentsByResponseId(r.ResponseId)
                        .Select(a => new AttachmentViewModel
                        {
                            AttachmentId = a.AttachmentId,
                            ResponseId = a.ResponseId,
                            FileData = a.File != null ? Convert.ToBase64String(a.File) : null
                        }).ToList()
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitFeedback(int ticketId, bool feedback)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            if (userRole != "User")
            {
                return RedirectToAction("Index", "Home");
            }

            var ticket = _ticketService.GetTicketById(ticketId);

            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }

            if (!ticket.IsFeedbackOffered)  
            {
                ticket.Feedback = feedback;
                ticket.IsFeedbackOffered = true;
                ticket.Status = "Closed"; 

                _ticketService.UpdateTicket(ticket);
            }

            TempData["SuccessMessage"] = "Feedback submitted.";
            return RedirectToAction("MyTickets", new { selectedTicketId = ticketId });
        }

        [HttpPost]
        public IActionResult AddResponse(int ticketId, string description, List<IFormFile> attachments)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                TempData["ErrorMessage"] = "Response cannot be empty.";
                return RedirectToAction("MyTickets", new { selectedTicketId = ticketId });
            }

            var ticket = _ticketService.GetTicketById(ticketId);

            if (ticket == null)
            {
                TempData["ErrorMessage"] = "Ticket not found.";
                return RedirectToAction("MyTickets");
            }

            // Prevent responses if the ticket is closed
            if (ticket.Status == "Closed")
            {
                TempData["ErrorMessage"] = "Responses are not allowed for closed tickets.";
                return RedirectToAction("MyTickets", new { selectedTicketId = ticketId });
            }

            try
            {
                var userIdString = HttpContext.Session.GetString("UserId");
                int userId = int.Parse(userIdString);

                var newResponse = new Response
                {
                    TicketId = ticketId,
                    Sender = userId,
                    Description = description,
                    CreatedTime = DateTime.Now
                };

                // Save response and get its ID
                var responseId = _responseService.AddResponse(newResponse);

                // Handle attachments
                if (attachments != null && attachments.Any())
                {
                    foreach (var attachment in attachments.Take(5)) // Limit to 5 files
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

                TempData["SuccessMessage"] = "Response sent.";
                return RedirectToAction("MyTickets", new { selectedTicketId = ticketId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding response: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to add the response. Please try again.";
                return RedirectToAction("MyTickets", new { selectedTicketId = ticketId });
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
        public IActionResult Delete(int ticketId)
        {
            if (ticketId <= 0)
            {
                return BadRequest("Invalid ticket ID.");
            }

            try
            {
                _ticketService.DeleteTicket(ticketId);
                _responseService.DeleteResponsesByTicketId(ticketId);
                TempData["SuccessMessage"] = "Ticket has been deleted";

                return RedirectToAction("MyTickets");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting ticket: {ex.Message}");
            }
        }

    }
}