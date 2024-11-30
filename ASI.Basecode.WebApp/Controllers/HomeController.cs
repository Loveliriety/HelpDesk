using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : ControllerBase<HomeController>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>

        private readonly ITicketService _ticketService;
        private readonly ICategoryService _categoryService;
        public HomeController(  ITicketService ticketService,
                                ICategoryService categoryService,
                                IHttpContextAccessor httpContextAccessor,
                                ILoggerFactory loggerFactory,
                                IConfiguration configuration,
                                IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _ticketService = ticketService;
            _categoryService = categoryService; 
        }

        /// <summary>
        /// Returns Home View.
        /// </summary>
        /// <returns> Home View </returns>
        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole == "User")
            {
                return RedirectToAction("Index", "Users");
            }

            var (isSuccess, tickets) = _ticketService.GetAllTickets();

            if (!isSuccess || tickets == null || !tickets.Any())
            {
                ViewBag.InProgressCount = 0;
                ViewBag.OpenCount = 0;
                ViewBag.SolvedCount = 0;
                ViewBag.ClosedCount = 0;
                ViewBag.TotalTicketsCount = 0;
                ViewBag.CategoryCounts = new Dictionary<string, int>();
                ViewBag.CategoryNames = new List<string>();  

                return View(new List<TicketPageViewModel>());
            }

            var inProgressCount = tickets.Count(t => t.Status == "In Progress");
            var openCount = tickets.Count(t => t.Status == "Open");
            var solvedCount = tickets.Count(t => t.Status == "Solved");
            var closedCount = tickets.Count(t => t.Status == "Closed");
            var totalTicketsCount = tickets.Count();

            var categoryCounts = tickets
                .GroupBy(t => t.Category) 
                .ToDictionary(g => g.Key, g => g.Count());

            var categories = _categoryService.GetAllCategories();

            var categoryCountDictionary = categoryCounts
                .ToDictionary(
                    kv => categories.FirstOrDefault(c => c.CategoryId == kv.Key)?.CategoryName ?? $"Category {kv.Key}",
                    kv => kv.Value
                );

            ViewBag.InProgressCount = inProgressCount;
            ViewBag.OpenCount = openCount;
            ViewBag.SolvedCount = solvedCount;
            ViewBag.ClosedCount = closedCount;
            ViewBag.TotalTicketsCount = totalTicketsCount;
            ViewBag.CategoryCounts = categoryCountDictionary;
            ViewBag.CategoryNames = categories.Select(c => c.CategoryName).ToList();

            return View(tickets);
        }






    }
}
