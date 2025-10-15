namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a completed trade involving the user.
    /// </summary>
    /// <remarks>
    /// Returned by the <c>GET /users/{userId}/trade-history</c> endpoint.  
    /// This DTO summarizes a completed trade, including buyer, seller, and pricing details.
    /// </remarks>
    public class UserTradeHistoryResponse
    {
        /// <summary>
        /// The unique identifier of the completed trade.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The type of trade that occurred.
        /// </summary>
        /// <remarks>
        /// Possible values include:
        /// <list type="bullet">
        /// <item><c>FOR_PRICE</c> – trade completed with currency</item>
        /// <item><c>FOR_CARD</c> – trade completed via card exchange</item>
        /// </list>
        /// </remarks>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// The identifier of the user who sold or offered the card.
        /// </summary>
        public Guid SellerUserId { get; set; }

        /// <summary>
        /// The identifier of the card sold or traded by the seller.
        /// </summary>
        public Guid SellerCardId { get; set; }

        /// <summary>
        /// The identifier of the user who purchased or received the card.
        /// </summary>
        public Guid BuyerUserId { get; set; }

        /// <summary>
        /// The identifier of the card traded by the buyer, if applicable.
        /// </summary>
        /// <remarks>
        /// This field is <c>null</c> for trades that involved a purchase rather than an exchange.
        /// </remarks>
        public Guid? BuyerCardId { get; set; }

        /// <summary>
        /// The final price paid by the buyer in a <c>FOR_PRICE</c> trade.
        /// </summary>
        /// <remarks>
        /// Set to <c>0</c> for card-for-card trades.
        /// </remarks>
        public int Price { get; set; }

        /// <summary>
        /// The date and time when the trade was completed.
        /// </summary>
        public DateTime ExecutedDate { get; set; }
    }

    /// <summary>
    /// Represents a paginated list of completed trades involving a user.
    /// </summary>
    /// <remarks>
    /// Returned by the <c>GET /users/{userId}/trade-history</c> endpoint.  
    /// Includes pagination information and a list of past trades where the user was a buyer or seller.
    /// </remarks>



    public class UserTradeHistoryListResponse
    {
        /// <summary>
        /// The list of completed trades involving the user.
        /// </summary>
        public IEnumerable<UserTradeHistoryResponse> Trades { get; set; } = new List<UserTradeHistoryResponse>();

        /// <summary>
        /// The total number of completed trades that match the query.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// The maximum number of trades returned in this page.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// The number of trades skipped before the current page.
        /// </summary>
        public int Offset { get; set; }
    }
}
