namespace SkyBooker.AuthService.DTOs
{
    public class UserProfileResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = string.Empty;
        public string? PassportNumber { get; set; }
        public string? Nationality { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}