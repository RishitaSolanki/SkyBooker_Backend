using System.ComponentModel.DataAnnotations;

namespace SkyBooker.AuthService.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Role { get; set; } = "PASSENGER"; // PASSENGER, AIRLINE_STAFF, ADMIN
    }
}