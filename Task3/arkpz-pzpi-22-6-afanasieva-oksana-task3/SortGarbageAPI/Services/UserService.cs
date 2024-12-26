using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;
using BCrypt.Net;

namespace SortGarbageAPI.Services
{

    /// <summary>
    /// Service for managing users
    /// </summary>
    public class UserService
    {
        private readonly SortGarbageDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the UserService
        /// </summary>
        /// <param name="dbContext">Database context for user operations</param>
        public UserService(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all users from the database
        /// </summary>
        /// <returns>Collection of all users</returns>
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        /// <summary>
        /// Checks if an email already exists in the database
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <returns>True if email exists, false otherwise</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        /// <summary>
        /// Retrieves a specific user by their id
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns>The user if found, null otherwise</returns>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        /// <summary>
        /// Creates a new user in the database
        /// </summary>
        /// <param name="user">The user to create</param>
        /// <returns>The created user with assigned id</returns>
        public async Task<User> CreateUserAsync(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Updates an existing user in the database
        /// </summary>
        /// <param name="id">The id of the user to update</param>
        /// <param name="updatedData">The updated user data</param>
        /// <returns>True if update was successful, false if user wasn't found</returns>
        public async Task<bool> UpdateUserAsync(int id, User updatedData)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) 
            { 
                return false; 
            }

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

        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        /// <param name="id">The id of the user to delete</param>
        /// <returns>True if deletion was successful, false if user wasn't found</returns>
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}