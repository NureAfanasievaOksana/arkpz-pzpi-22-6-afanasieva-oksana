namespace SortGarbageAPI.Models
{
    using Microsoft.EntityFrameworkCore;

    public class SortGarbageDbContext : DbContext
    {
        public SortGarbageDbContext(DbContextOptions<SortGarbageDbContext> options)
            : base(options)
        {
        }
        public DbSet<Container> Containers { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<SensorData> SensorData { get; set; }

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
