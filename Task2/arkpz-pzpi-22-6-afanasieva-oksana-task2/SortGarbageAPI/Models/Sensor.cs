namespace SortGarbageAPI.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="Sensor" />
    /// </summary>
    public class Sensor
    {
        #region Properties

        [JsonIgnore]
        public Container? Container { get; set; }

        [ForeignKey("Container")]
        public int ContainerId { get; set; }

        [Key]
        public int SensorId { get; set; }

        [Required]
        public SensorType Type { get; set; }

        #endregion
    }
}
