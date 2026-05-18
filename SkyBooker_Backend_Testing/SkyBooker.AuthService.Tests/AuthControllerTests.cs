using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SkyBooker.AuthService.Controllers;
using SkyBooker.AuthService.DTOs;
using SkyBooker.AuthService.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBooker.AuthService.Tests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        // --- 1. Register ---
        [Test]
        public async Task Register_ReturnsOkResult_WithAuthResponse()
        {
            var registerRequest = new RegisterRequest { Email = "test@test.com", Password = "password" };
            var authResponse = new AuthResponse { Token = "jwt_token", Role = "User", UserId = Guid.NewGuid() };

            _mockAuthService.Setup(s => s.RegisterAsync(registerRequest)).ReturnsAsync(authResponse);

            var result = await _controller.Register(registerRequest);
            var okResult = result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        // --- 2. Login ---
        [Test]
        public async Task Login_ReturnsOkResult_WithAuthResponse()
        {
            var loginRequest = new LoginRequest { Email = "test@test.com", Password = "password" };
            var authResponse = new AuthResponse { Token = "jwt_token", Role = "User", UserId = Guid.NewGuid() };

            _mockAuthService.Setup(s => s.LoginAsync(loginRequest)).ReturnsAsync(authResponse);

            var result = await _controller.Login(loginRequest);
            var okResult = result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        // --- 3. GetUserById ---
        [Test]
        public async Task GetUserById_ReturnsOkResult_WhenUserExists()
        {
            var userId = Guid.NewGuid();
            var userProfileResponse = new UserProfileResponse { UserId = userId, Email = "test@test.com", FullName = "Test User", Role = "User" };

            _mockAuthService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(userProfileResponse);

            var result = await _controller.GetUserById(userId);
            var okResult = result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();

            _mockAuthService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync((UserProfileResponse)null);

            var result = await _controller.GetUserById(userId);
            var notFoundResult = result as NotFoundObjectResult;

            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        }

        // --- 4. GetUserByEmail ---
        [Test]
        public async Task GetUserByEmail_ReturnsOkResult_WhenUserExists()
        {
            var email = "test@test.com";
            var userProfileResponse = new UserProfileResponse { UserId = Guid.NewGuid(), Email = email, FullName = "Test User", Role = "User" };

            _mockAuthService.Setup(s => s.GetUserByEmailAsync(email)).ReturnsAsync(userProfileResponse);

            var result = await _controller.GetUserByEmail(email);
            var okResult = result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetUserByEmail_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var email = "nonexistent@test.com";

            _mockAuthService.Setup(s => s.GetUserByEmailAsync(email)).ReturnsAsync((UserProfileResponse)null);

            var result = await _controller.GetUserByEmail(email);
            var notFoundResult = result as NotFoundObjectResult;

            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        }

        // --- 5. SearchUsers ---
        [Test]
        public async Task SearchUsers_ReturnsOkResult_WhenTermIsValid()
        {
            var term = "test";
            var users = new List<UserProfileResponse> { new UserProfileResponse { Email = "test@test.com" } };

            _mockAuthService.Setup(s => s.SearchUsersAsync(term)).ReturnsAsync(users);

            var result = await _controller.SearchUsers(term);
            var okResult = result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task SearchUsers_ReturnsBadRequest_WhenTermIsEmpty()
        {
            var term = "";

            var result = await _controller.SearchUsers(term);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }

        // --- 6. DeleteUser ---
        [Test]
        public async Task DeleteUser_ReturnsOkResult_WhenSuccessful()
        {
            var userId = Guid.NewGuid();

            _mockAuthService.Setup(s => s.DeleteUserAsync(userId)).ReturnsAsync(true);

            var result = await _controller.DeleteUser(userId);
            var okResult = result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();

            _mockAuthService.Setup(s => s.DeleteUserAsync(userId)).ReturnsAsync(false);

            var result = await _controller.DeleteUser(userId);
            var notFoundResult = result as NotFoundObjectResult;

            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        }

        // --- 7. UpdateUser ---
        [Test]
        public async Task UpdateUser_ReturnsOkResult_WhenSuccessful()
        {
            var userId = Guid.NewGuid();
            var request = new UpdateUserRequest();

            _mockAuthService.Setup(s => s.UpdateUserAsync(userId, request)).ReturnsAsync(true);

            var result = await _controller.UpdateUser(userId, request);
            var okResult = result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task UpdateUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var request = new UpdateUserRequest();

            _mockAuthService.Setup(s => s.UpdateUserAsync(userId, request)).ReturnsAsync(false);

            var result = await _controller.UpdateUser(userId, request);
            var notFoundResult = result as NotFoundObjectResult;

            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        }

        // --- 8. UpdatePassword ---
        [Test]
        public async Task UpdatePassword_ReturnsOkResult_WhenSuccessful()
        {
            var userId = Guid.NewGuid();
            var request = new UpdatePasswordRequest();

            _mockAuthService.Setup(s => s.UpdatePasswordAsync(userId, request)).ReturnsAsync(true);

            var result = await _controller.UpdatePassword(userId, request);
            var okResult = result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task UpdatePassword_ReturnsBadRequest_WhenCredentialsAreInvalid()
        {
            var userId = Guid.NewGuid();
            var request = new UpdatePasswordRequest();

            _mockAuthService.Setup(s => s.UpdatePasswordAsync(userId, request)).ReturnsAsync(false);

            var result = await _controller.UpdatePassword(userId, request);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }

        // --- 9. UpdateUserRole ---
        [Test]
        public async Task UpdateUserRole_ReturnsOkResult_WhenSuccessful()
        {
            var userId = Guid.NewGuid();
            var role = "Admin";

            _mockAuthService.Setup(s => s.UpdateUserRoleAsync(userId, role)).ReturnsAsync(true);

            var result = await _controller.UpdateUserRole(userId, role);
            var okResult = result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task UpdateUserRole_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var role = "Admin";

            _mockAuthService.Setup(s => s.UpdateUserRoleAsync(userId, role)).ReturnsAsync(false);

            var result = await _controller.UpdateUserRole(userId, role);
            var notFoundResult = result as NotFoundObjectResult;

            Assert.That(notFoundResult, Is.Not.Null);
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
        }
    }
}
