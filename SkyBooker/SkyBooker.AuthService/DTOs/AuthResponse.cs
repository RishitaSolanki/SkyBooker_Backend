namespace SkyBooker.AuthService.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}