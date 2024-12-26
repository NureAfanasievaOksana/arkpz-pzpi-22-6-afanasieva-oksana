using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;

namespace SortGarbageAPI.Services
{
    public class NotificationService
    {
        private readonly SortGarbageDbContext _dbContext;

        public NotificationService(SortGarbageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _dbContext.Notifications.ToListAsync();
        }

        public async Task<Notification?> GetNotificationByIdAsync(int id)
        {
            return await _dbContext.Notifications.FindAsync(id);
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _dbContext.Notifications.FindAsync(id);
            if (notification == null) return false;

            _dbContext.Notifications.Remove(notification);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
