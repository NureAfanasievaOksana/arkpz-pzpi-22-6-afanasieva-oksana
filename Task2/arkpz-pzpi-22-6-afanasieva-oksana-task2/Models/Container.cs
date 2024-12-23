using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SortGarbage.Models
{
    public class Container
    {
        [Key]
        public int ContainerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        public ContainerType Type { get; set; }

        [Required]
        public float MaxSize { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}