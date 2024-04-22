using Core.Entities;
using Core.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        
        private readonly PostgressDbContext _context;

        public UserRepository(PostgressDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyList<User>> GetListOfUsersAsync() //Usage Not Recomended, use paging
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IReadOnlyList<User>> GetPagedListOfUsersAsync(int pageIndex, int pageSize)
        {
            
            return await _context.Users.Skip(pageIndex* pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int Id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string Username)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(user => user.Username == Username);
            return user;
        }

        public async Task<User> UpdateUserAsync(User newUser)
        {
            User user = await _context.Users.FirstAsync(u => u.Id == newUser.Id);
            user.Role = newUser.Role;
            user.Password = newUser.Password;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
