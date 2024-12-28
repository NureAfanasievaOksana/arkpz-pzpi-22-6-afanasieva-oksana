using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;
using SortGarbageAPI.Services;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;

namespace SortGarbageAPI.Controllers
{
    /// <summary>
    /// Controller handling user authentication and authorization
    /// </summary>
    [ApiController]
    [Route("/auth")]
    public class AuthorizationController : ControllerBase
    {
        private readonly AuthorizationService _authorizationService;
        private readonly SortGarbageDbContext _dbContext;

        /// <summary>
        /// Constructor for AuthorizationController
        /// </summary>
        /// <param name="dbContext">Database context for user data access</param>
        /// <param name="authorizationService">Service for authorization operations</param>
        public AuthorizationController(SortGarbageDbContext dbContext, AuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Handles user login and generates authentication token
        /// </summary>
        /// <param name="loginData">User login credentials</param>
        /// <returns>JWT token if authentication successful, unauthorized response if failed</returns>
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

    /// <summary>
    /// Data transfer object for login credentials
    /// </summary>
    public class LoginData
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}