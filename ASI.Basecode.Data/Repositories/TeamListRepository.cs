using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class TeamListRepository : BaseRepository, ITeamListRepository
    {
        private readonly AsiBasecodeDBContext _dbContext;

        public TeamListRepository(AsiBasecodeDBContext dbContext, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<TeamList> ViewTeams()
        {
            return this.GetDbSet<TeamList>().ToList();
        }

        public void AddTeam(TeamList team)
        {
            this.GetDbSet<TeamList>().Add(team);
            UnitOfWork.SaveChanges();
        }

        public void UpdateTeam(TeamList team)
        {
            this.GetDbSet<TeamList>().Update(team);
            UnitOfWork.SaveChanges();
        }

        public void DeleteTeam(TeamList team)
        {
            this.GetDbSet<TeamList>().Remove(team);
            UnitOfWork.SaveChanges();
        }
    }
}
