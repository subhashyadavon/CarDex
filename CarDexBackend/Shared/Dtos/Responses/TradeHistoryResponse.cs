namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a paginated list of completed trades in the user’s trade history.
    /// </summary>
    /// <remarks>
    /// This response is typically returned from <c>GET /trades/history</c> or <c>GET /users/{userId}/trade-history</c>.  
    /// It includes all finalized trades where the user participated as a buyer or seller,  
    /// along with pagination metadata.
    /// </remarks>
    public class TradeHistoryResponse
    {
        /// <summary>
        /// The collection of completed trades matching the query parameters.
        /// </summary>
        /// <remarks>
        /// Each entry provides details such as the trade type, participants, price, and execution date.
        /// </remarks>
        public IEnumerable<CompletedTradeResponse> Trades { get; set; } = new List<CompletedTradeResponse>();

        /// <summary>
        /// The total number of completed trades that match the query.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// The maximum number of results returned in this page.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// The number of records skipped before this page.
        /// </summary>
        public int Offset { get; set; }
    }
}
