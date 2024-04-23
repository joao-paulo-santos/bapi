using Core.Entities;
using Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{

    public class UserService : IUserService
    {
        const int maxPageSize = 15;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<User?> ChangeUserRoleAsync(User user, Role newRole)
        {
            user.Role = newRole;
            return await _unitOfWork.UserRepository.UpdateUserAsync(user);

        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            return await _unitOfWork.UserRepository.DeleteUserAsync(user);
        }

        public async Task<IReadOnlyList<User>> GetListOfUsersAsync()
        {
            return await _unitOfWork.UserRepository.GetListOfUsersAsync();
        }

        public async Task<IReadOnlyList<User>> GetPagedListOfUsersAsync(int pageIndex, int pageSize)
        {
            int size = pageSize > maxPageSize ? maxPageSize : pageSize;
            size = size > 0 ? size : 5;
            return await _unitOfWork.UserRepository.GetPagedListOfUsersAsync(pageIndex, size);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
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

        public async Task<User?> RegisterUserAsync(string Username, string Password)
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

            return await _unitOfWork.UserRepository.AddUserAsync(newUser);
        }

    }
}
