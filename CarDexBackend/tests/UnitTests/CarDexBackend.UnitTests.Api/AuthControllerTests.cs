using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using CarDexBackend.Api.Controllers;
using CarDexBackend.Services;
using CarDexBackend.Shared.Dtos.Requests;
using CarDexBackend.Shared.Dtos.Responses;
using System;
using System.Threading.Tasks;

namespace CarDexBackend.UnitTests.Api.Controllers
{
    /// <summary>
    /// Contains unit tests for the <see cref="AuthController"/> class.
    /// </summary>
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthControllerTests"/> class with mocked dependencies.
        /// </summary>
        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        // ===== SUCCESSES =====

        /// <summary>
        /// Verifies that <see cref="AuthController.Register"/> returns a <see cref="CreatedAtActionResult"/> when registration is successful.
        /// </summary>
        [Fact]
        public async Task Register_Succeeds()
        {
            var request = new RegisterRequest { Username = "testuser", Password = "password" };
            var user = new UserResponse { Id = Guid.NewGuid(), Username = "testuser" };

            _mockAuthService.Setup(s => s.Register(It.IsAny<RegisterRequest>())).ReturnsAsync(user);

            var result = await _controller.Register(request);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var value = Assert.IsType<UserResponse>(createdResult.Value);
            Assert.Equal(user.Username, value.Username);
        }

        /// <summary>
        /// Verifies that <see cref="AuthController.Login"/> returns an <see cref="OkObjectResult"/> when valid credentials are provided.
        /// </summary>
        [Fact]
        public async Task Login_Succeeds()
        {
            var request = new LoginRequest { Username = "testuser", Password = "password" };
            var loginResponse = new LoginResponse { AccessToken = "abc123" };

            _mockAuthService.Setup(s => s.Login(It.IsAny<LoginRequest>())).ReturnsAsync(loginResponse);

            var result = await _controller.Login(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.Equal("abc123", value.AccessToken);
        }

        /// <summary>
        /// Verifies that <see cref="AuthController.Logout"/> returns a <see cref="NoContentResult"/>.
        /// </summary>
        [Fact]
        public async Task Logout_Succeeds()
        {
            _mockAuthService.Setup(s => s.Logout(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            var result = await _controller.Logout();

            Assert.IsType<NoContentResult>(result);
        }

        // ===== FAILURES =====

        /// <summary>
        /// Verifies that <see cref="AuthController.Register"/> returns a <see cref="ConflictObjectResult"/> when the username already exists.
        /// </summary>
        [Fact]
        public async Task Register_Conflicts()
        {
            var request = new RegisterRequest { Username = "duplicate", Password = "123" };

            _mockAuthService.Setup(s => s.Register(It.IsAny<RegisterRequest>())).ThrowsAsync(new InvalidOperationException("Username already exists."));

            var result = await _controller.Register(request);

            var conflict = Assert.IsType<ConflictObjectResult>(result);
            var error = Assert.IsType<ErrorResponse>(conflict.Value);
            Assert.Equal("Username already exists.", error.Message);
        }

        /// <summary>
        /// Ensures Register returns 400 BadRequest for unknown exceptions.
        /// </summary>
        [Fact]
        public async Task Register_BadRequest()
        {
            var request = new RegisterRequest { Username = "broken", Password = "test" };

            _mockAuthService.Setup(s => s.Register(It.IsAny<RegisterRequest>())).ThrowsAsync(new Exception("Bad request"));

            var result = await _controller.Register(request);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var error = Assert.IsType<ErrorResponse>(badRequest.Value);
            Assert.Equal("Bad request", error.Message);
        }

        /// <summary>
        /// Verifies that <see cref="AuthController.Login"/> returns an <see cref="UnauthorizedObjectResult"/> when credentials are invalid.
        /// </summary>
        [Fact]
        public async Task Login_Unauthorized()
        {
            var request = new LoginRequest { Username = "wrong", Password = "invalid" };

            _mockAuthService.Setup(s => s.Login(It.IsAny<LoginRequest>())).ThrowsAsync(new UnauthorizedAccessException("Invalid credentials."));

            var result = await _controller.Login(request);

            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            var error = Assert.IsType<ErrorResponse>(unauthorized.Value);
            Assert.Equal("Invalid credentials.", error.Message);
        }
    }
}
