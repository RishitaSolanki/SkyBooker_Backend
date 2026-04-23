using Microsoft.AspNetCore.Mvc;
using SkyBooker.AuthService.DTOs;
using SkyBooker.AuthService.Services;

namespace SkyBooker.AuthService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found" });
            return Ok(user);
        }

        [HttpGet("user/email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _authService.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound(new { message = "User not found" });
            return Ok(user);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string term)
        {
            if (string.IsNullOrEmpty(term))
                return BadRequest(new { message = "Search term is required" });
            
            var users = await _authService.SearchUsersAsync(term);
            return Ok(users);
        }

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var result = await _authService.DeleteUserAsync(userId);
            if (!result)
                return NotFound(new { message = "User not found" });
            return Ok(new { message = "User deleted successfully" });
        }

        [HttpPut("user/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserRequest request)
        {
            var result = await _authService.UpdateUserAsync(userId, request);
            if (!result)
                return NotFound(new { message = "User not found" });
            return Ok(new { message = "User updated successfully" });
        }

        [HttpPut("user/{userId}/password")]
        public async Task<IActionResult> UpdatePassword(Guid userId, [FromBody] UpdatePasswordRequest request)
        {
            var result = await _authService.UpdatePasswordAsync(userId, request);
            if (!result)
                return BadRequest(new { message = "Invalid user or old password" });
            return Ok(new { message = "Password updated successfully" });
        }
    }
}