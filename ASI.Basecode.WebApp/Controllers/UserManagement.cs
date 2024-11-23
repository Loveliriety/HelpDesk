using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserManagementController(IUserService userService, 
                                        IHttpContextAccessor httpContextAccessor,
                                        ITeamService teamService)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _teamService = teamService;
        }

        // GET: UserManagement
        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole == "User")
            {
                return RedirectToAction("Index", "Users");
            }

            var users = _userService.GetUsers().Where(u => u.IsActive);


            var result = _teamService.GetTeams();
            var teams = result.Item2;

            ViewBag.Teams = new SelectList(teams, "TeamId", "TeamName");

            if (userRole == "Admin")
            {
                users = users.Where(u => u.Role != "Superadmin");
            }

            return View(users);
        }

        // GET: UserManagement/Create
        public IActionResult Create()
        {
            var result = _teamService.GetTeams();
            var teams = result.Item2;

            ViewBag.Teams = new SelectList(teams, "TeamId", "TeamName");

            return View();
        }

        // POST: UserManagement/Create
        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _userService.AddUser(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // POST: UserManagement/Edit
        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (user != null)
            {
                _userService.UpdateUser(user);
                TempData["SuccessMessage"] = "User has been updated";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(User user)
        {
            if (user != null)
            {
                _userService.DeleteUser(user);
            }

            return RedirectToAction("Index");
        }
    }
}