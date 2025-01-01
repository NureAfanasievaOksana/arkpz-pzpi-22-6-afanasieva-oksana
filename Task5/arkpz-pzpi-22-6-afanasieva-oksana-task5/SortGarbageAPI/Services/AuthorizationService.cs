using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Services
{
    /// <summary>
    /// Service responsible for handling JWT token generation and authorization
    /// </summary>
    public class AuthorizationService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the AuthorizationService
        /// </summary>
        /// <param name="configuration">Application configuration containing JWT settings</param>
        public AuthorizationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JWT token for user authentication
        /// </summary>
        /// <param name="userId">The unique id of the user</param>
        /// <param name="role">The role assigned to the user</param>
        /// <returns>JWT token string that can be used for authentication</returns>
        public string GenerateToken(int userId, UserRole role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Role, ((int)role).ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}