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
using System.Linq;
using System.Security.Claims;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UsersController : ControllerBase<UsersController>
    {
        private readonly ICategoryService _categoryService;
        private readonly ITicketService _ticketService;
        public UsersController(ICategoryService categoryService,
                                ITicketService ticketService,
                                IHttpContextAccessor httpContextAccessor,
                                ILoggerFactory loggerFactory,
                                IConfiguration configuration,
                                IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _categoryService = categoryService;
            _ticketService = ticketService;
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
                    Assignee = ticket.Assignee,
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
                    Assignee = null, 
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

        public IActionResult MyTickets(string status = null)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "User")
            {
                return RedirectToAction("Index", "Home");
            }
            var userEmail = HttpContext.Session.GetString("Email");

            // Redirect non-User roles to the Home page
            if (userRole != "User")
            {
                return RedirectToAction("Index", "Home");
            }

            // Fetch tickets from the service
            var (success, tickets) = _ticketService.GetAllTickets();
            if (!success)
            {
                return BadRequest("Failed to fetch tickets.");
            }

            // Define allowed statuses for filtering
            var allowedStatuses = new[] { "Open", "In Progress", "Resolved", "Closed" };

            // Filter tickets for the current user and allowed statuses
            tickets = tickets
                .Where(ticket => ticket.RequesterEmail == userEmail && allowedStatuses.Contains(ticket.Status))
                .ToList();

            // Apply status filter if specified
            if (!string.IsNullOrEmpty(status) && allowedStatuses.Contains(status))
            {
                tickets = tickets.Where(ticket => ticket.Status == status).ToList();
                ViewData["SelectedStatus"] = status;
            }

            // Prepare status counts for the view
            ViewData["StatusCounts"] = allowedStatuses.ToDictionary(
                stat => stat,
                stat => tickets.Count(ticket => ticket.Status == stat)
            );

            // Map tickets to the view model
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

    }
}