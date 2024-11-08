using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Services.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserManagementRepository _userManagementRepository;

        public UserManagementService(IUserManagementRepository userManagementRepository)
        {
            this._userManagementRepository = userManagementRepository;
        }

        public List<UserManagement> GetUsers()
        {
            var users = _userManagementRepository.ViewUsers().ToList(); 
            return users;
        }

        public void AddUser(UserManagement user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            var newUser = new UserManagement
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                CreatedAt = DateTime.UtcNow
            };

            _userManagementRepository.AddUser(newUser);
        }

        public void UpdateUser(UserManagement user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            _userManagementRepository.UpdateUser(user);
        }

        public void DeleteUser(UserManagement user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            _userManagementRepository.DeleteUser(user);
        }
    }
}
