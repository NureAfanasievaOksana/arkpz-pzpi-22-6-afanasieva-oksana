using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;
using BCrypt.Net;

namespace SortGarbageAPI.Services
{
    public class UserService
    {
        private readonly SortGarbageDbContext _dbContext;

        public UserService(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(int id, User updatedData)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return false;

            user.Username = updatedData.Username;
            user.Address = updatedData.Address;
            user.Email = updatedData.Email;
            if (!string.IsNullOrEmpty(updatedData.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updatedData.Password);
            }
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return false;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
