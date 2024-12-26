namespace SortGarbageAPI.Models
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="SortGarbageDbContext" />
    /// </summary>
    public class SortGarbageDbContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SortGarbageDbContext"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="DbContextOptions{SortGarbageDbContext}"/></param>
        public SortGarbageDbContext(DbContextOptions<SortGarbageDbContext> options)
            : base(options)
        {
        }

        #endregion

        #region Properties

        public DbSet<Container> Containers { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<SensorData> SensorData { get; set; }

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<User> Users { get; set; }

        #endregion
    }
}
