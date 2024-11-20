using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class TeamRepository : BaseRepository, ITeamRepository
    {
        private readonly AsiBasecodeDBContext _dbContext;

        public TeamRepository(AsiBasecodeDBContext dbContext, IUnitOfWork unitOfWork) : base(unitOfWork) 
        { 
            _dbContext = dbContext;
        }

        public void AddTeam(Team team)
        { 
            this.GetDbSet<Team>().Add(team);    
            UnitOfWork.SaveChanges();
        }

        public void UpdateTeam(Team team)
        { 
            var existingTeam = _dbContext.Teams.FirstOrDefault(team => team.TeamId == team.TeamId);

            if (existingTeam == null)
            {
                throw new Exception("Team not found!");
            }

            existingTeam = team;

            UnitOfWork.SaveChanges();
        }

        public void DeleteTeam(Team team)
        {
            _dbContext.Remove(team);
            UnitOfWork.SaveChanges();
        }

        public Team GetTeamById(int? id)
        {
            var team = _dbContext.Teams.FirstOrDefault(team => team.TeamId == id);

            if (team == null)
            {
                throw new Exception("Team not found!");
            }

            return team;
        }

        public IEnumerable<Team>? GetTeams()
        {
            return _dbContext.Teams.ToList();
        }

        public IEnumerable<Team> GetAllTeams()
        {
            return this.GetDbSet<Team>();
        }
    }
}
