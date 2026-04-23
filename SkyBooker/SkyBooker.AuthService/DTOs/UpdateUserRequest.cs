using System.ComponentModel.DataAnnotations;

namespace SkyBooker.AuthService.DTOs
{
    public class UpdateUserRequest
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
