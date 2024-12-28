using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SortGarbageAPI.Models;
using SortGarbageAPI.Services;

namespace SortGarbageAPI.Controllers
{
    /// <summary>
    /// Controller responsible for managing user-related operations
    /// </summary>
    [ApiController]
    [Route("/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        /// <summary>
        /// Initializes a new instance of the UserController class
        /// </summary>
        /// <param name="userService">The service for managing user operations</param>
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves all users from the system
        /// </summary>
        /// <returns>A collection of all users</returns>
        [HttpGet]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Creates a new user in the system
        /// </summary>
        /// <param name="user">The user data to create</param>
        /// <returns>The created user data</returns>
        [HttpPost]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (await _userService.EmailExistsAsync(user.Email))
            {
                return BadRequest("There is already a user with this email");
            }

            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser);
        }

        /// <summary>
        /// Retrieves a specific user by their id
        /// </summary>
        /// <param name="id">The id of the user to retrieve</param>
        /// <returns>The requested user data</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Updates an existing user's information
        /// </summary>
        /// <param name="id">The id of the user to update</param>
        /// <param name="updatedData">The updated user data</param>
        /// <returns>A success message if the update was successful</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedData)
        {
            if (!await _userService.UpdateUserAsync(id, updatedData))
            {
                return NotFound();
            }
            return Ok("User data updated successfully");
        }

        /// <summary>
        /// Deletes a user from the system
        /// </summary>
        /// <param name="id">The id of the user to delete</param>
        /// <returns>A success message if the deletion was successful</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!await _userService.DeleteUserAsync(id))
            {
                return NotFound();
            }
            return Ok("User data deleted successfully");
        }
    }
}