using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    
    public class UserService : IUserService
    {
        const int maxPageSize = 15;
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> ChangeUserRoleAsync(User user, Role newRole)
        {
            user.Role = newRole;
            await _userRepository.UpdateUserAsync(user);
            return await _userRepository.GetUserByIdAsync(user.Id);
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            await _userRepository.DeleteUserAsync(user);
            return await _userRepository.GetUserByIdAsync(user.Id) == null;
        }

        public async Task<IReadOnlyList<User>> GetListOfUsersAsync()
        {
            return await _userRepository.GetListOfUsersAsync();
        }

        public async Task<IReadOnlyList<User>> GetPagedListOfUsersAsync(int pageIndex, int pageSize)
        {
            int size = pageSize > maxPageSize ? maxPageSize : pageSize;
            size = size > 0 ? size : 5;
            return await _userRepository.GetPagedListOfUsersAsync(pageIndex, size);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        public bool VerifyPassword(User user, string Password)
        {
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(Password));
            }
            string stringHash = Convert.ToHexString(hash);
            return stringHash.Equals(user.Password, StringComparison.CurrentCultureIgnoreCase);
        }

        public async Task<User> RegisterUserAsync(string Username, string Password)
        {
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(Password));
            }
            string stringHash = Convert.ToHexString(hash);

            User newUser = new User
            {
                CreatedDate = DateTime.UtcNow,
                Username = Username,
                Password = stringHash,
                Role = Role.User
            };

            await _userRepository.AddUserAsync(newUser);
            return await _userRepository.GetUserByUsernameAsync(Username);
        }

    }
}
