using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SortGarbage.Models
{
    public class Sensor
    {
        [Key]
        public int SensorId { get; set; }

        [Required]
        public SensorType Type { get; set; }

        [ForeignKey("Container")]
        public int ContainerId { get; set; }

        [JsonIgnore]
        public Container Container { get; set; }
    }
}