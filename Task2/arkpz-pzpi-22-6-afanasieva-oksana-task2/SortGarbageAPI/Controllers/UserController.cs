using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;
using BCrypt.Net;

namespace SortGarbageAPI.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserController : ControllerBase
    {
        private readonly SortGarbageDbContext _dbContext;

        public UserController(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if(await _dbContext.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest("There is already a user with this email");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedData)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            user.Username = updatedData.Username;
            user.Address = updatedData.Address;
            user.Email = updatedData.Email;
            if (string.IsNullOrEmpty(updatedData.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updatedData.Password);
            }
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return Ok("User data updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return Ok("User data deleted successfully");
        }
    }
}
