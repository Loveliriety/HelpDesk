using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        // GET: UserManagement
        public IActionResult Index()
        {
            var users = _userManagementService.GetUsers();
            return View(users);
        }

        // GET: UserManagement/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserManagement/Create
        [HttpPost]
        public IActionResult Create(UserManagement user)
        {
            if (ModelState.IsValid)
            {
                _userManagementService.AddUser(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // POST: UserManagement/Edit
        [HttpPost]
        public IActionResult Edit(UserManagement user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _userManagementService.GetUsers().FirstOrDefault(u => u.UserMID == user.UserMID);
                if (existingUser != null)
                {
                    existingUser.Name = user.Name;
                    existingUser.Email = user.Email;
                    existingUser.Role = user.Role;

                    if (!string.IsNullOrWhiteSpace(user.Password))
                    {
                        existingUser.Password = user.Password;
                    }

                    existingUser.UpdatedAt = DateTime.UtcNow;

                    _userManagementService.UpdateUser(existingUser);
                    return RedirectToAction("Index");
                }
            }
            return View("Index", _userManagementService.GetUsers());
        }

        public IActionResult Delete(int id)
        {
            var user = _userManagementService.GetUsers().FirstOrDefault(u => u.UserMID == id);
            if (user != null)
            {
                _userManagementService.DeleteUser(user);
            }
            return RedirectToAction("Index");
        }
    }
}
