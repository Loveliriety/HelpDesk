using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository,
                            IMapper mapper,
                            ITeamRepository teamRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _teamRepository = teamRepository;
        }

        public LoginResult AuthenticateUser(string userId, string password, ref User user)
        {
            user = new User();
            var passwordKey = PasswordManager.EncryptPassword(password);
            user = _repository.GetUsers().Where(x => x.UserId == userId &&
                                                     x.Password == passwordKey).FirstOrDefault();

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }

        public void AddUser(UserViewModel model)
        {
            var user = new User();
            if (!_repository.UserExists(model.UserId))
            {
                _mapper.Map(model, user);
                user.Password = PasswordManager.EncryptPassword(model.Password);
                user.CreatedTime = DateTime.Now;
                user.UpdatedTime = DateTime.Now;
                user.CreatedBy = System.Environment.UserName;
                user.UpdatedBy = System.Environment.UserName;

                _repository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }

        public List<User> GetUsers()
        {
            var users = _repository.GetUsers().ToList();
            return users;
        }


        public void AddUser(User user)
        {
            // Validate if TeamId Exists before adding user
            var team = _teamRepository.GetTeamById(user.TeamId);
            if (team == null)
            {
                throw new InvalidDataException("invalid TeamId provided!");
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            var newUser = new User
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Password = PasswordManager.EncryptPassword(user.Password),
                Role = user.Role,
                TeamId = user.TeamId,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                CreatedBy = Environment.UserName,
                UpdatedBy = Environment.UserName
            };

            _repository.AddUser(newUser);
        }

        public void UpdateUser(User user)
        {
            // Validate if TeamId Exists before adding user
            var team = _teamRepository.GetTeamById(user.TeamId);
            if (team == null)
            {
                throw new InvalidDataException("invalid TeamId provided!");
            }

            var existingUser = _repository.GetUsers().FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                if (!string.IsNullOrEmpty(user.Password))
                {
                    existingUser.Password = PasswordManager.EncryptPassword(user.Password); // Optional: Encrypt password if necessary
                }
                existingUser.Role = user.Role;
                existingUser.TeamId = user.TeamId;
                existingUser.IsActive = user.IsActive;
                existingUser.UpdatedTime = DateTime.Now;
                existingUser.UpdatedBy = Environment.UserName;

                _repository.UpdateUser(existingUser);
            }
        }

        public void DeleteUser(User user)
        {
            var existingUser = _repository.GetUsers().FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.IsActive = user.IsActive == false;
                existingUser.UpdatedTime = DateTime.Now;
                existingUser.UpdatedBy = Environment.UserName;

                _repository.DeleteUser(existingUser);
            }
        }
    }
}
