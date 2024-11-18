using ASI.Basecode.Data.Models;
using System.Collections.Generic;

namespace ASI.Basecode.Services.Interfaces
{
    public interface ITeamListService
    {
        List<TeamList> GetTeams();
        void AddTeam(TeamList team);
        void UpdateTeam(TeamList team);
        void DeleteTeam(TeamList team);
    }
}