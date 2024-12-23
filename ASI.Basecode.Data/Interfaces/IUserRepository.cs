﻿using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetUsers();
        bool UserExists(int userId);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        User GetUserById(int? id);
    }
}
