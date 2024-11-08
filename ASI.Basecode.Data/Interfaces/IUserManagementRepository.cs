using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IUserManagementRepository
    {
        IEnumerable<UserManagement> ViewUsers();
        void AddUser(UserManagement user);
        void UpdateUser(UserManagement user);
        void DeleteUser(UserManagement user);
    }
}
