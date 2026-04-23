using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyBooker.AuthService.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required]
        public string Role { get; set; } = "PASSENGER";

        public string? Provider { get; set; }   // LOCAL / GOOGLE

        public bool IsActive { get; set; } = true;

        public string? PassportNumber { get; set; }

        public string? Nationality { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}