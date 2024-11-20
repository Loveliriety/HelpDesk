using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface ITeamService
    {
        public (bool, IEnumerable<Team>) GetTeams();
        List<Team> GetAllTeams();
        public void AddTeam(Team team);
        public Team GetTeamById(int id);
        public void UpdateTeam(Team team);
        public void DeleteTeam(int id);
        public string GetTeamNameById(int? id);

    }
}
