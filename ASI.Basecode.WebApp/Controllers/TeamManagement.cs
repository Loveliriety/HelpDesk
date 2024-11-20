using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Services;
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
    public class TeamManagementController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TeamManagementController(ITeamService teamService,
                              IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _teamService = teamService;
        }

        // GET: TeamManagement
        public IActionResult Index()
        {
            var teams = _teamService.GetAllTeams();

            return View(teams);
        }

        // GET: TeamManagement/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TeamManagement/Create
        [HttpPost]
        public IActionResult Create(Team team)
        {
            if (ModelState.IsValid)
            {
                _teamService.AddTeam(team);
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // POST: TeamManagement/Edit
        [HttpPost]
        public IActionResult Edit(Team team)
        {
            if (team != null)
            {
                _teamService.UpdateTeam(team);
                TempData["SuccessMessage"] = "Team has been updated";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int TeamId)
        {
            var team = _teamService.GetTeamById(TeamId);

            if (team != null)
            {
                _teamService.DeleteTeam(TeamId);
            }

            return RedirectToAction("Index");
        }
    }
}