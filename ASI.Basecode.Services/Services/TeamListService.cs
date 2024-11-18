using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Services.Services
{
    public class TeamListService : ITeamListService
    {
        private readonly ITeamListRepository _teamListRepository;

        public TeamListService(ITeamListRepository teamListRepository)
        {
            this._teamListRepository = teamListRepository;
        }

        public List<TeamList> GetTeams()
        {
            var team = _teamListRepository.ViewTeams().ToList();
            return team;
        }

        public void AddTeam(TeamList team)
        {
            if (team == null)
            {
                throw new ArgumentNullException(nameof(team), "team cannot be null");
            }

            var newTeam = new TeamList
            {
                Name = team.Name,
                Company = team.Company,
                Tier = team.Tier,
                Manager = team.Manager,
                CreatedAt = DateTime.UtcNow
            };

            _teamListRepository.AddTeam(newTeam);
        }

        public void UpdateTeam(TeamList team)
        {
            if (team == null)
            {
                throw new ArgumentNullException(nameof(team), "team cannot be null");
            }

            _teamListRepository.UpdateTeam(team);
        }

        public void DeleteTeam(TeamList team)
        {
            if (team == null)
            {
                throw new ArgumentNullException(nameof(team), "team cannot be null");
            }

            _teamListRepository.DeleteTeam(team);
        }
    }
}
