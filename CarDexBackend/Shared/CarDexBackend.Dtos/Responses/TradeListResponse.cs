namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a paginated list of active or open trades.
    /// </summary>
    /// <remarks>
    /// This DTO is typically returned from endpoints such as <c>GET /trades</c>.  
    /// It includes summary information about each trade along with pagination metadata.
    /// </remarks>
    public class TradeListResponse
    {
        /// <summary>
        /// The collection of trade summaries.
        /// </summary>
        /// <remarks>
        /// Each trade entry contains basic information such as type, price, and owner.  
        /// Detailed trade information can be retrieved using <c>GET /trades/{tradeId}</c>.
        /// </remarks>
        public IEnumerable<TradeResponse> Trades { get; set; } = new List<TradeResponse>();

        /// <summary>
        /// The total number of trades that match the query parameters.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// The maximum number of results returned in this page.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// The number of results skipped before this page.
        /// </summary>
        public int Offset { get; set; }
    }
}
