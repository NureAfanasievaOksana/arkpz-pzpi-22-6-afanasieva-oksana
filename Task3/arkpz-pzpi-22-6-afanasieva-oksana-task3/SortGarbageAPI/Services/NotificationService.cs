using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Services
{
    /// <summary>
    /// Service for managing notifications
    /// </summary>
    public class NotificationService
    {
        private readonly SortGarbageDbContext _dbContext;
        private readonly EmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the NotificationService
        /// </summary>
        /// <param name="dbContext">Database context for notification operations</param>
        public NotificationService(SortGarbageDbContext dbContext, EmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        /// <summary>
        /// Retrieves all notifications from the database
        /// </summary>
        /// <returns>Collection of all notifications</returns>
        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _dbContext.Notifications.ToListAsync();
        }

        /// <summary>
        /// Creates a new notification in the database
        /// </summary>
        /// <param name="notification">The notification to create</param>
        /// <returns>The created notification with assigned id</returns>
        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            var user = await _dbContext.Users.FindAsync(notification.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            await _emailService.SendEmailAsync(user.Email, notification.Subject, notification.Message);

            notification.Timestamp = DateTime.UtcNow;
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
            return notification;
        }

        /// <summary>
        /// Retrieves a specific notification by its id
        /// </summary>
        /// <param name="id">The notification id</param>
        /// <returns>The notification if found, null otherwise</returns>
        public async Task<Notification?> GetNotificationByIdAsync(int id)
        {
            return await _dbContext.Notifications.FindAsync(id);
        }

        /// <summary>
        /// Deletes a notification from the database
        /// </summary>
        /// <param name="id">The id of the notification to delete</param>
        /// <returns>True if deletion was successful, false if notification wasn't found</returns>
        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _dbContext.Notifications.FindAsync(id);
            if (notification == null)
            {
                return false;
            }

            _dbContext.Notifications.Remove(notification);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves all notifications for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user to get notifications for</param>
        /// <returns>Collection of notifications for the specified user</returns>
        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _dbContext.Notifications.Where(n => n.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Retrieves all notifications for a specific date
        /// </summary>
        /// <param name="date">The date to get notifications for</param>
        /// <returns>Collection of notifications created on the specified date</returns>
        public async Task<IEnumerable<Notification>> GetNotificationsByDateAsync(DateTime date)
        {
            return await _dbContext.Notifications.Where(n => n.Timestamp.Date == date.Date).ToListAsync();
        }
    }
}