using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IUserService
    {
        LoginResult AuthenticateUser(string userid, string password, ref User user);
        List<User> GetUsers();
        void AddUser(UserViewModel model);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
