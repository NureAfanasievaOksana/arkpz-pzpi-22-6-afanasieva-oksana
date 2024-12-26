namespace SortGarbageAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="User" />
    /// </summary>
    public class User
    {
        #region Properties

        [StringLength(100)]
        public string? Address { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(60)]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        #endregion
    }
}
