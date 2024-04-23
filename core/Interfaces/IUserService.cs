using Core.Entities;

namespace Core.Interfaces
{
    public interface IUserService
    {
        Task<IReadOnlyList<User>> GetListOfUsersAsync();
        Task<IReadOnlyList<User>> GetPagedListOfUsersAsync(int pageIndex, int pageSize);

        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> RegisterUserAsync(string Username, string Password);
        bool VerifyPassword(User user, string Password);
        Task<User?> ChangeUserRoleAsync(User user, Role newRole);
        Task<bool> DeleteUserAsync(User user);
    }
}
