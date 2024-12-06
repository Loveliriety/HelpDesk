using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {

        }

        public IQueryable<User> GetUsers()
        {
            return this.GetDbSet<User>().Where(u => u.IsActive);
        }

        public User GetUserById(int? id)
        {
            var user = this.GetDbSet<User>().FirstOrDefault(user => user.UserId == id);

            //if (user == null)
            //{
            //    throw new Exception("Team not found!");
            //}

            return user;
        }

        public bool UserExists(int userId)
        {
            return this.GetDbSet<User>().Any(x => x.UserId == userId);
        }

        public void AddUser(User user)
        {
            this.GetDbSet<User>().Add(user);
            UnitOfWork.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            this.GetDbSet<User>().Update(user);
            UnitOfWork.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            this.GetDbSet<User>().Update(user);
            UnitOfWork.SaveChanges();
        }

    }
}
