using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class UserManagementRepository : BaseRepository, IUserManagementRepository
    {
        private readonly AsiBasecodeDBContext _dbContext;

        public UserManagementRepository(AsiBasecodeDBContext dbContext, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<UserManagement> ViewUsers()
        {
            return this.GetDbSet<UserManagement>().ToList();
        }

        public void AddUser(UserManagement user)
        {
            this.GetDbSet<UserManagement>().Add(user);
            UnitOfWork.SaveChanges();
        }

        public void UpdateUser(UserManagement user)
        {
            this.GetDbSet<UserManagement>().Update(user);
            UnitOfWork.SaveChanges();
        }

        public void DeleteUser(UserManagement user)
        {
            this.GetDbSet<UserManagement>().Remove(user);
            UnitOfWork.SaveChanges();
        }
    }
}
