using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SortGarbageAPI.Models;
using SortGarbageAPI.Services;

namespace SortGarbageAPI.Controllers
{
    /// <summary>
    /// Controller responsible for managing notifications
    /// </summary>
    [Route("/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the NotificationController class
        /// </summary>
        /// <param name="notificationService">The service for managing notification operations</param>
        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Retrieves all notifications from the system
        /// </summary>
        /// <returns>A collection of all notifications</returns>
        [HttpGet]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetNotifications()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return Ok(notifications);
        }

        /// <summary>
        /// Creates a new notification in the system
        /// </summary>
        /// <param name="notification">The notification data to create</param>
        /// <returns>The created notification data</returns>
        [HttpPost]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
        {
            var createdNotification = await _notificationService.CreateNotificationAsync(notification);
            return CreatedAtAction(nameof(GetNotificationById), new { id = createdNotification.NotificationId }, createdNotification);
        }

        /// <summary>
        /// Retrieves a specific notification by its id
        /// </summary>
        /// <param name="id">The id of the notification to retrieve</param>
        /// <returns>The requested notification data</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }

        /// <summary>
        /// Deletes a notification from the system
        /// </summary>
        /// <param name="id">The id of the notification to delete</param>
        /// <returns>A success message if the deletion was successful</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            if (!await _notificationService.DeleteNotificationAsync(id))
            {
                return NotFound();
            }
            return Ok("Notification deleted successfully");
        }

        /// <summary>
        /// Retrieves all notifications for a specific user
        /// </summary>
        /// <param name="userId">The id of the user to retrieve notifications for</param>
        /// <returns>A collection of notifications for the specified user</returns>
        [HttpGet("users/{userId}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetNotificationsByUserId(int userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }

        /// <summary>
        /// Retrieves all notifications for a specific date
        /// </summary>
        /// <param name="date">The date to retrieve notifications for</param>
        /// <returns>A collection of notifications for the specified date</returns>
        [HttpGet("date/{date}")]
        [Authorize(Roles = "2, 3")]
        public async Task<IActionResult> GetNotificationsByDate(DateTime date)
        {
            var notifications = await _notificationService.GetNotificationsByDateAsync(date);
            return Ok(notifications);
        }
    }
}