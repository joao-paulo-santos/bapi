using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserService
    {
        Task<IReadOnlyList<User>> GetListOfUsersAsync();
        Task<IReadOnlyList<User>> GetPagedListOfUsersAsync(int pageIndex, int pageSize);
        
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> RegisterUserAsync(string Username, string Password);
        bool VerifyPassword(User user, string Password);
        Task<User> ChangeUserRoleAsync(User user, Role newRole);
        Task<bool> DeleteUserAsync(User user);
    }
}
