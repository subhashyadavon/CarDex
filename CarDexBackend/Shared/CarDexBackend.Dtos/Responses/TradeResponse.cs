namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a trade created by a user.
    /// </summary>
    /// <remarks>
    /// This DTO contains summary-level information about a trade offer in the marketplace.  
    /// A trade can either be a <c>FOR_CARD</c> (card-for-card) or <c>FOR_PRICE</c> (card-for-currency) exchange.  
    /// Returned when a trade is created or listed in open trades.
    /// </remarks>
    public class TradeResponse
    {
        /// <summary>
        /// The unique identifier for the trade.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The type of trade being made.
        /// </summary>
        /// <remarks>
        /// Possible values: <c>FOR_CARD</c> or <c>FOR_PRICE</c>.  
        /// Determines whether the trade is for another card or for currency.
        /// </remarks>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the user who created the trade.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The username of the user who created the trade.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the card being offered for trade.
        /// </summary>
        public Guid CardId { get; set; }

        /// <summary>
        /// The asking price of the card, if this is a <c>FOR_PRICE</c> trade.
        /// </summary>
        /// <remarks>
        /// May be <c>null</c> if the trade type is <c>FOR_CARD</c>.
        /// </remarks>
        public int? Price { get; set; }

        /// <summary>
        /// The ID of the desired card, if this is a <c>FOR_CARD</c> trade.
        /// </summary>
        /// <remarks>
        /// May be <c>null</c> if the trade type is <c>FOR_PRICE</c>.
        /// </remarks>
        public Guid? WantCardId { get; set; }

        /// <summary>
        /// The timestamp when the trade was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
