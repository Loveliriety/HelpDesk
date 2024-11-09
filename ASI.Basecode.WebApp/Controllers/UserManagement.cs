using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IUserService _userService;

        public UserManagementController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: UserManagement
        public IActionResult Index()
        {
            var users = _userService.GetUsers().Where(u => u.IsActive);
            return View(users);
        }

        // GET: UserManagement/Create
        public IActionResult Create()
        {
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
                TempData["SuccessMessage"] = "User has been edited";
            }

            return RedirectToAction("Index");
        }

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