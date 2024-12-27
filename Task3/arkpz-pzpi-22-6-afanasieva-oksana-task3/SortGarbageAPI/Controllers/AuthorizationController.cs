using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;
using SortGarbageAPI.Services;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;

namespace SortGarbageAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("/auth")]
    public class AuthorizationController : ControllerBase
    {
        private readonly AuthorizationService _authorizationService;
        private readonly SortGarbageDbContext _dbContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"> </param>
        public AuthorizationController(SortGarbageDbContext dbContext, AuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _authorizationService = authorizationService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginData loginData)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == loginData.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginData.Password, user.Password))
            {
                return Unauthorized("Invalid email or password");
            }

            var token = _authorizationService.GenerateToken(user.UserId, user.Role);
            return Ok(new { Token = token });
        }
    }

    public class LoginData
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}