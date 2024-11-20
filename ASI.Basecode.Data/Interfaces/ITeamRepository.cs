using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface ITeamRepository
    {
        public IEnumerable<Team> GetTeams();
        public IEnumerable<Team> GetAllTeams();
        public void AddTeam(Team team);
        public Team GetTeamById(int? id);
        public void UpdateTeam(Team team);
        public void DeleteTeam(Team team);
    }
}
