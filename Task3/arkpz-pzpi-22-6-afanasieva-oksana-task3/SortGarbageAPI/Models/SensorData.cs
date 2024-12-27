using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SortGarbageAPI.Models
{
    public class SensorData
    {
        [Key]
        public int SensorDataId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public float Fullness { get; set; }

        [Required]
        public float Temperature { get; set; }

        [Required]
        public float Wetness { get; set; }

        [ForeignKey("Sensor")]
        public int SensorId { get; set; }

        [JsonIgnore]
        public Sensor? Sensor { get; set; }
    }
}