using SkyBooker.AuthService.DTOs;
using SkyBooker.AuthService.Entities;

namespace SkyBooker.AuthService.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<UserProfileResponse?> GetUserByIdAsync(Guid userId);
        Task<UserProfileResponse?> GetUserByEmailAsync(string email);
        Task<List<UserProfileResponse>> SearchUsersAsync(string searchTerm);
        Task<List<UserProfileResponse>> GetAllUsersAsync(string role);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> UpdateUserAsync(Guid userId, UpdateUserRequest request);
        Task<bool> UpdatePasswordAsync(Guid userId, UpdatePasswordRequest request);
    }
}