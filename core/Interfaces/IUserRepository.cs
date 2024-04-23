using Core.Entities;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int Id);
        Task<User?> GetUserByUsernameAsync(string Username);
        Task<IReadOnlyList<User>> GetListOfUsersAsync();
        Task<IReadOnlyList<User>> GetPagedListOfUsersAsync(int pageIndex, int pageSize);
        Task<User?> AddUserAsync(User newUser);
        Task<User?> UpdateUserAsync(User newUser);
        Task<bool> DeleteUserAsync(User user);
    }
}
