namespace SortGarbageAPI.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="Notification" />
    /// </summary>
    public class Notification
    {
        #region Properties

        [Required]
        public string Message { get; set; }

        [Key]
        public int NotificationId { get; set; }

        [JsonIgnore]
        public SensorData? SensorData { get; set; }

        [ForeignKey("SensorData")]
        public int? SensorDataId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        #endregion
    }
}
