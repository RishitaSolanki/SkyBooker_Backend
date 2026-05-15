using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SkyBooker.AuthService.Data;
using SkyBooker.AuthService.DTOs;
using SkyBooker.AuthService.Entities;
using SkyBooker.AuthService.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SkyBooker.AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(IUserRepository userRepository,
                           IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email))
                throw new Exception("Email already exists");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                Role = request.Role ?? "PASSENGER"
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Token = token,
                Role = user.Role,
                UserId = user.UserId
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);

            if (user == null)
                throw new Exception("Invalid credentials");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid credentials");

            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Token = token,
                Role = user.Role,
                UserId = user.UserId
            };
        }

        public async Task<UserProfileResponse?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.FindByUserIdAsync(userId);
            if (user == null) return null;

            return MapToProfileResponse(user);
        }

        public async Task<UserProfileResponse?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null) return null;

            return MapToProfileResponse(user);
        }

        public async Task<List<UserProfileResponse>> SearchUsersAsync(string searchTerm)
        {
            var users = await _userRepository.SearchUsersAsync(searchTerm);
            return users.Select(MapToProfileResponse).ToList();
        }

        public async Task<List<UserProfileResponse>> GetAllUsersAsync(string role)
        {
            var users = await _userRepository.FindAllByRoleAsync(role);
            return users.Select(MapToProfileResponse).ToList();
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.FindByUserIdAsync(userId);
            if (user == null)
                return false;

            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(Guid userId, UpdateUserRequest request)
        {
            var user = await _userRepository.FindByUserIdAsync(userId);
            if (user == null)
                return false;

            user.FullName = request.FullName;
            user.Email = request.Email;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePasswordAsync(Guid userId, UpdatePasswordRequest request)
        {
            var user = await _userRepository.FindByUserIdAsync(userId);
            if (user == null)
                return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.OldPassword);

            if (result == PasswordVerificationResult.Failed)
                return false;

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserRoleAsync(Guid userId, string role)
        {
            var user = await _userRepository.FindByUserIdAsync(userId);
            if (user == null)
                return false;

            user.Role = role;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        private UserProfileResponse MapToProfileResponse(User user)
        {
            return new UserProfileResponse
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                PassportNumber = user.PassportNumber,
                Nationality = user.Nationality,
                CreatedAt = user.CreatedAt
            };
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? 
                    throw new InvalidOperationException("JWT SecretKey not configured")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToUpper())
            };

            var token = new JwtSecurityToken(
                issuer: "SkyBooker",
                audience: "SkyBooker",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}