namespace SortGarbageAPI.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="Container" />
    /// </summary>
    public class Container
    {
        #region Properties

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Key]
        public int ContainerId { get; set; }

        [Required]
        public float MaxSize { get; set; }

        [Required]
        public ContainerType Type { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        #endregion
    }
}
