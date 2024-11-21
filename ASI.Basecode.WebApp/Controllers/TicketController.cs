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
using System.Linq;

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

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    TempData["Error"] = "An error occurred while creating the ticket.";
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                TempData["Error"] = "Validation failed. Please check your inputs.";
            }

            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public IActionResult AddResponse(int ticketId, string sender, string description)
        {
            if (ticketId <= 0 || string.IsNullOrWhiteSpace(description))
            {
                TempData["ErrorMessage"] = "Invalid ticket ID or response description.";
                return RedirectToAction("ViewTicket", new { id = ticketId });
            }

            var newResponse = new Response
            {
                TicketId = ticketId,
                Sender = sender,
                Description = description,
                CreatedTime = DateTime.Now
            };

            try
            {
                _responseService.AddResponse(newResponse);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "There was an error adding the response.";
                return RedirectToAction("ViewTicket", new { id = ticketId });
            }

            return RedirectToAction("ViewTicket", new { id = ticketId });
        }

        [HttpGet]
        public IActionResult ViewTicket(int id)
        {
            var ticket = _ticketService.GetTicketById(id);

            if (ticket == null)
            {
                return NotFound();
            }

            var responses = _responseService.GetResponsesByTicketId(id);

            var model = new TicketPageViewModel
            {
                TicketId = ticket.TicketId,
                Subject = ticket.Subject,
                RequesterEmail = ticket.RequesterEmail,
                Category = ticket.Category,
                Status = ticket.Status,      
                Priority = ticket.Priority,  
                CreatedTime = ticket.CreatedTime,
                UpdatedTime = ticket.UpdatedTime,
                Responses = responses.Select(r => new ResponseViewModel
                {
                    ResponseId = r.ResponseId,
                    TicketId = r.TicketId,
                    Sender = r.Sender,
                    Description = r.Description,
                    CreatedTime = r.CreatedTime
                }).ToList()
            };

            return View(model);
        }

        // Index
        public IActionResult Index(int? categoryId)
        {
            var (success, tickets) = _ticketService.GetAllTickets();
            var categories = _categoryService.GetAllCategories();
            var users = _userService.GetUsers();

            if (!success)
            {
                return BadRequest();
            }

            // Filter tickets if a category is selected
            if (categoryId.HasValue)
            {
                tickets = tickets.Where(ticket => ticket.Category == categoryId.Value).ToList();
                ViewData["SelectedCategory"] = categoryId;
            }

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
        //working
    }
}
