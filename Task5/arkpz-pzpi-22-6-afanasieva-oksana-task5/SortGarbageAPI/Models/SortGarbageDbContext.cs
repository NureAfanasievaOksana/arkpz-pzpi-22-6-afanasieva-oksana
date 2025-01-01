using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SortGarbageAPI.Models
{
    public class SortGarbageDbContext : DbContext
    {
        IConfiguration _configuration;
        public SortGarbageDbContext(DbContextOptions<SortGarbageDbContext> options, IConfiguration configuration)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            _configuration = configuration;
        }
        public DbSet<Container> Containers { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<SensorData> SensorData { get; set; }

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
            base.OnConfiguring(optionsBuilder);
        }
    }
}