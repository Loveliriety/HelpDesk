using ASI.Basecode.Data.Models;
using System.Collections.Generic;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IUserManagementService
    {
        List<UserManagement> GetUsers();
        void AddUser(UserManagement user);
        void UpdateUser(UserManagement user);
        void DeleteUser(UserManagement user);
    }
}