using Microsoft.AspNetCore.Mvc;
using CarDexBackend.Shared.Dtos.Requests;
using CarDexBackend.Shared.Dtos.Responses;
using CarDexBackend.Services;

namespace CarDexBackend.Controllers
{
    /// <summary>
    /// Manages trade-related operations including creating, browsing, executing, and cancelling trades.
    /// </summary>
    /// <remarks>
    /// Supports both card-for-card and card-for-currency trades, along with browsing open and completed trade history.
    /// </remarks>
    [ApiController]
    [Route("trades")]
    public class TradesController : ControllerBase
    {
        private readonly ITradeService _tradeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TradesController"/> class.
        /// </summary>
        /// <param name="tradeService">Service responsible for trade management logic.</param>
        public TradesController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        /// <summary>
        /// Retrieves all open trades currently available in the marketplace.
        /// </summary>
        /// <param name="type">Optional filter for trade type (FOR_CARD or FOR_PRICE).</param>
        /// <param name="collectionId">Optional filter by card collection.</param>
        /// <param name="grade">Optional card grade filter (FACTORY, LIMITED_RUN, NISMO).</param>
        /// <param name="minPrice">Minimum trade price filter.</param>
        /// <param name="maxPrice">Maximum trade price filter.</param>
        /// <param name="vehicleId">Optional filter for a specific vehicle card.</param>
        /// <param name="wantCardId">Optional filter for trades requesting a specific card.</param>
        /// <param name="sortBy">Sort order for results (price_asc, date_desc, etc.).</param>
        /// <param name="limit">Maximum number of results per page (default 50).</param>
        /// <param name="offset">Number of results to skip for pagination.</param>
        /// <returns>A paginated list of open trades.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(TradeListResponse), 200)]
        public async Task<IActionResult> GetOpenTrades(
            [FromQuery] string? type, [FromQuery] Guid? collectionId, [FromQuery] string? grade,
            [FromQuery] int? minPrice, [FromQuery] int? maxPrice, [FromQuery] Guid? vehicleId,
            [FromQuery] Guid? wantCardId, [FromQuery] string? sortBy = "date_desc",
            [FromQuery] int limit = 50, [FromQuery] int offset = 0)
        {
            var trades = await _tradeService.GetOpenTrades(type, collectionId, grade, minPrice, maxPrice, vehicleId, wantCardId, sortBy, limit, offset);
            return Ok(trades);
        }

        /// <summary>
        /// Retrieves details for a specific open trade by its unique ID.
        /// </summary>
        /// <param name="tradeId">The unique identifier of the trade.</param>
        /// <returns>Detailed information about the specified trade.</returns>
        [HttpGet("{tradeId:guid}")]
        [ProducesResponseType(typeof(TradeDetailedResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> GetOpenTradeById(Guid tradeId)
        {
            try
            {
                var trade = await _tradeService.GetOpenTradeById(tradeId);
                return Ok(trade);
            }
            catch (KeyNotFoundException ex)
            {
                // Trade not found in open trades
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new trade listing.
        /// </summary>
        /// <param name="request">Trade creation details (offered card, trade type, price or wanted card).</param>
        /// <returns>201 Created with trade details if successful, 400 Bad Request if invalid input.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TradeResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> CreateTrade([FromBody] TradeCreateRequest request)
        {
            try
            {
                var result = await _tradeService.CreateTrade(request);
                return CreatedAtAction(nameof(GetOpenTradeById), new { tradeId = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                // Invalid trade request or missing required field
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }

        /// <summary>
        /// Cancels an existing open trade and returns the offered card to the owner.
        /// </summary>
        /// <param name="tradeId">The unique identifier of the trade to cancel.</param>
        /// <returns>204 No Content if successful, 404 Not Found if trade does not exist.</returns>
        [HttpDelete("{tradeId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> CancelTrade(Guid tradeId)
        {
            try
            {
                await _tradeService.DeleteTrade(tradeId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                // Trade not found or already completed
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
        }

        /// <summary>
        /// Executes an existing trade, transferring cards or currency between users.
        /// </summary>
        /// <param name="tradeId">The unique identifier of the trade to execute.</param>
        /// <param name="request">Optional buyer trade data (e.g. buyer card ID for card-for-card trades).</param>
        /// <returns>
        /// 200 OK with completed trade details and generated rewards.
        /// 400 Bad Request for invalid or missing input.
        /// 404 Not Found if the trade no longer exists.
        /// </returns>
        [HttpPost("{tradeId:guid}/execute")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> ExecuteTrade(Guid tradeId, [FromBody] TradeExecuteRequest? request)
        {
            try
            {
                var result = await _tradeService.ExecuteTrade(tradeId, request);
                return Ok(new
                {
                    completed_trade = result.CompletedTrade,
                    seller_reward = result.SellerReward,
                    buyer_reward = result.BuyerReward
                });
            }
            catch (KeyNotFoundException ex)
            {
                // Trade ID not found or already executed
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                // Invalid execution request (e.g. buyer card missing for FOR_CARD trade)
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves the trade history of completed trades.
        /// </summary>
        /// <param name="userId">Optional filter to retrieve trades by a specific user.</param>
        /// <param name="limit">Maximum number of results per page (default 50).</param>
        /// <param name="offset">Number of results to skip for pagination.</param>
        /// <returns>A paginated list of completed trades.</returns>
        [HttpGet("history")]
        [ProducesResponseType(typeof(TradeHistoryResponse), 200)]
        public async Task<IActionResult> GetTradeHistory([FromQuery] Guid? userId, [FromQuery] int limit = 50, [FromQuery] int offset = 0)
        {
            var result = await _tradeService.GetTradeHistory(userId, limit, offset);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves details for a specific completed trade.
        /// </summary>
        /// <param name="tradeId">The unique identifier of the completed trade.</param>
        /// <returns>Detailed information about the completed trade.</returns>
        [HttpGet("history/{tradeId:guid}")]
        [ProducesResponseType(typeof(CompletedTradeResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> GetCompletedTrade(Guid tradeId)
        {
            try
            {
                var trade = await _tradeService.GetCompletedTradeById(tradeId);
                return Ok(trade);
            }
            catch (KeyNotFoundException ex)
            {
                // Completed trade record not found
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
        }
    }
}
