using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class TeamListController : Controller
    {
        private readonly ITeamListService _teamListService;

        public TeamListController(ITeamListService teamListService)
        {
            _teamListService = teamListService;
        }

        // GET: TeamList
        public IActionResult Index()
        {
            var teams = _teamListService.GetTeams();
            return View(teams);
        }

        // GET: TeamList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TeamList/Create
        [HttpPost]
        public IActionResult Create(TeamList team)
        {
            if (ModelState.IsValid)
            {
                _teamListService.AddTeam(team);
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // POST: TeamList/Edit
        [HttpPost]
        public IActionResult Edit(TeamList team)
        {
            if (ModelState.IsValid)
            {
                var existingTeam = _teamListService.GetTeams().FirstOrDefault(t => t.TeamID == team.TeamID);
                if (existingTeam != null)
                {
                    existingTeam.Name = team.Name;
                    existingTeam.Company = team.Company;
                    existingTeam.Tier = team.Tier;
                    existingTeam.Manager = team.Manager;
                    existingTeam.UpdatedAt = DateTime.UtcNow;

                    _teamListService.UpdateTeam(existingTeam);
                    return RedirectToAction("Index");
                }
            }
            return View("Index", _teamListService.GetTeams());
        }

        public IActionResult Delete(int id)
        {
            var team = _teamListService.GetTeams().FirstOrDefault(t => t.TeamID == id);
            if (team != null)
            {
                _teamListService.DeleteTeam(team);
            }
            return RedirectToAction("Index");
        }
    }
}
