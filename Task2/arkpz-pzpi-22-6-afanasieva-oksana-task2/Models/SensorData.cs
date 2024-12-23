using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SortGarbage.Models
{
    public class SensorData
    {
        [Key]
        public int SensorDataId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public float Value { get; set; }

        [ForeignKey("Sensor")]
        public int SensorId { get; set; }

        [JsonIgnore]
        public Sensor Sensor { get; set; }
    }
}