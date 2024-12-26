namespace SortGarbageAPI.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="SensorData" />
    /// </summary>
    public class SensorData
    {
        #region Properties

        [JsonIgnore]
        public Sensor? Sensor { get; set; }

        [Key]
        public int SensorDataId { get; set; }

        [ForeignKey("Sensor")]
        public int SensorId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public float Value { get; set; }

        #endregion
    }
}
