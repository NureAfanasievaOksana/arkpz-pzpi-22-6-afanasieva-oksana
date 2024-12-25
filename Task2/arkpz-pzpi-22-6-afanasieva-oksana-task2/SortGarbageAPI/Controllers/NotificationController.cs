using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Controllers
{
    [Route("/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly SortGarbageDbContext _dbContext;

        public NotificationController(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var notifications = await _dbContext.Notifications.ToListAsync();
            return Ok(notifications);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
        {
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotificationById), new { id = notification.NotificationId }, notification);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _dbContext.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return Ok(notification);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _dbContext.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            _dbContext.Notifications.Remove(notification);
            await _dbContext.SaveChangesAsync();
            return Ok("Notification deleted successfully");
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetNotificationsByUserId(int userId)
        {
            var notification = await _dbContext.Notifications.Where(n => n.UserId == userId).ToListAsync();
            return Ok(notification);
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetNotificationsByDate(DateTime date)
        {
            var notifications = await _dbContext.Notifications.Where(n => n.Timestamp.Date == date.Date).ToListAsync();
            return Ok(notifications);
        }
    }
}