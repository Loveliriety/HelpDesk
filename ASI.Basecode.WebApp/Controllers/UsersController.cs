using ASI.Basecode.Services.Interfaces;
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

namespace ASI.Basecode.WebApp.Controllers
{
    public class UsersController : ControllerBase<UsersController>
    {
        private readonly ICategoryService _categoryService;
        public UsersController(ICategoryService categoryService,
                                IHttpContextAccessor httpContextAccessor,
                                ILoggerFactory loggerFactory,
                                IConfiguration configuration,
                                IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "User")
            {
                return RedirectToAction("Index", "Home");
            }

            // Fetch the team names
            var categories = _categoryService.GetAllCategories();

            // Prepare SelectLists for team
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View();
        }

        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "User")
            {
                return RedirectToAction("Index", "Home");
            }

            // Fetch the team names
            var categories = _categoryService.GetAllCategories();

            // Prepare SelectLists for team
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View();
        }

        public IActionResult MyTickets()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "User")
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}