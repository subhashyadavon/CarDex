using Microsoft.AspNetCore.Mvc;
using CarDexBackend.Shared.Dtos.Requests;
using CarDexBackend.Shared.Dtos.Responses;
using CarDexBackend.Services;

namespace CarDexBackend.Api.Controllers
{
    /// <summary>
    /// Handles user authentication and account management operations.
    /// </summary>
    /// <remarks>
    /// Provides endpoints for user registration, login, and logout.
    /// In development, this uses a mock auth service for local testing.
    /// </remarks>
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service used for user management.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="request">The registration request containing username and password.</param>
        /// <returns>
        /// 201 Created on success, 400 Bad Request for invalid input,
        /// or 409 Conflict if the username already exists.
        /// </returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 409)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _authService.Register(request);
                return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                // Username already exists
                return Conflict(new ErrorResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Invalid input or unknown error
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }
        
        /// <summary>
        /// Authenticates a user and issues an access token.
        /// </summary>
        /// <param name="request">The login request containing credentials.</param>
        /// <returns>
        /// 200 OK with login response on success, or 401 Unauthorized if credentials are invalid.
        /// </returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.Login(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Invalid credentials
                return Unauthorized(new ErrorResponse { Message = ex.Message });
            }
        }

        /// <summary>
        /// Logs out the current user by invalidating their session/token.
        /// </summary>
        /// <remarks>
        /// Currently a placeholder for mock environments.
        /// The real implementation will extract the user ID from the authorization token.
        /// </remarks>
        /// <returns>204 No Content if logout is successful.</returns>
        [HttpPost("logout")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        public async Task<IActionResult> Logout()
        {
            // TODO: Replace Guid.Empty with real user ID when token authorization is added
            await _authService.Logout(Guid.Empty);
            return NoContent();
        }
    }
}
