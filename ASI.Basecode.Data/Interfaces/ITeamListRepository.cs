using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface ITeamListRepository
    {
        IEnumerable<TeamList> ViewTeams();
        void AddTeam(TeamList team);
        void UpdateTeam(TeamList team);
        void DeleteTeam(TeamList team);
    }
}
