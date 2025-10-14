using CarDexBackend.Services;
using CarDexBackend.Shared.Dtos.Requests;
using CarDexBackend.Shared.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CarDexBackend.Controllers
{
    /// <summary>
    /// Manages user-related operations such as profile management, inventory, trades, and rewards.
    /// </summary>
    /// <remarks>
    /// Provides endpoints for retrieving user profiles, updating account details, and viewing
    /// user-specific data such as owned cards, packs, trades, trade history, and rewards.
    /// </remarks>
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">Service responsible for user and inventory management.</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves a public user profile by ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>Public profile information for the specified user.</returns>
        [HttpGet("{userId:guid}")]
        [ProducesResponseType(typeof(UserPublicResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> GetUserProfile(Guid userId)
        {
            try
            {
                var result = await _userService.GetUserProfile(userId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                // User not found in mock data
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
        }

        /// <summary>
        /// Updates a user's profile information.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to update.</param>
        /// <param name="request">Profile update request containing new username or password.</param>
        /// <returns>The updated user profile.</returns>
        [HttpPatch("{userId:guid}")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> UpdateUserProfile(Guid userId, [FromBody] UserUpdateRequest request)
        {
            try
            {
                var result = await _userService.UpdateUserProfile(userId, request);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                // Attempted to update a non-existent user
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all cards owned by a specific user.
        /// </summary>
        /// <param name="userId">User identifier whose cards to fetch.</param>
        /// <param name="collectionId">Optional filter for collection ID.</param>
        /// <param name="grade">Optional filter for card grade.</param>
        /// <param name="limit">Number of results per page (default 50).</param>
        /// <param name="offset">Number of results to skip for pagination.</param>
        /// <returns>A paginated list of the user's cards.</returns>
        [HttpGet("{userId:guid}/cards")]
        [ProducesResponseType(typeof(UserCardListResponse), 200)]
        public async Task<IActionResult> GetUserCards(Guid userId, [FromQuery] Guid? collectionId, [FromQuery] string? grade, [FromQuery] int limit = 50, [FromQuery] int offset = 0)
        {
            var result = await _userService.GetUserCards(userId, collectionId, grade, limit, offset);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all unopened packs owned by a specific user.
        /// </summary>
        /// <param name="userId">User identifier whose packs to fetch.</param>
        /// <param name="collectionId">Optional filter for collection ID.</param>
        /// <returns>A list of unopened packs owned by the user.</returns>
        [HttpGet("{userId:guid}/packs")]
        [ProducesResponseType(typeof(UserPackListResponse), 200)]
        public async Task<IActionResult> GetUserPacks(Guid userId, [FromQuery] Guid? collectionId)
        {
            var result = await _userService.GetUserPacks(userId, collectionId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all open trades created by a specific user.
        /// </summary>
        /// <param name="userId">User identifier whose trades to fetch.</param>
        /// <param name="type">Optional filter for trade type (FOR_CARD or FOR_PRICE).</param>
        /// <returns>A list of open trades owned by the user.</returns>
        [HttpGet("{userId:guid}/trades")]
        [ProducesResponseType(typeof(UserTradeListResponse), 200)]
        public async Task<IActionResult> GetUserTrades(Guid userId, [FromQuery] string? type)
        {
            var result = await _userService.GetUserTrades(userId, type);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves completed trade history for a specific user.
        /// </summary>
        /// <param name="userId">User identifier whose trade history to fetch.</param>
        /// <param name="role">Optional role filter (seller, buyer, or all).</param>
        /// <param name="limit">Number of results per page (default 50).</param>
        /// <param name="offset">Number of results to skip for pagination.</param>
        /// <returns>A paginated list of completed trades.</returns>
        [HttpGet("{userId:guid}/trade-history")]
        [ProducesResponseType(typeof(UserTradeHistoryListResponse), 200)]
        public async Task<IActionResult> GetUserTradeHistory(Guid userId, [FromQuery] string role = "all", [FromQuery] int limit = 50, [FromQuery] int offset = 0)
        {
            var result = await _userService.GetUserTradeHistory(userId, role, limit, offset);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves pending or claimed rewards for a specific user.
        /// </summary>
        /// <param name="userId">User identifier whose rewards to fetch.</param>
        /// <param name="claimed">Whether to include already claimed rewards (default false).</param>
        /// <returns>A list of rewards associated with the user.</returns>
        [HttpGet("{userId:guid}/rewards")]
        [ProducesResponseType(typeof(UserRewardListResponse), 200)]
        public async Task<IActionResult> GetUserRewards(Guid userId, [FromQuery] bool claimed = false)
        {
            var result = await _userService.GetUserRewards(userId, claimed);
            return Ok(result);
        }
    }
}
