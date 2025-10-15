namespace CarDexBackend.Shared.Dtos.Requests
{
    /// <summary>
    /// Represents a request to execute a trade between two users.
    /// </summary>
    /// <remarks>
    /// This request is used when a buyer accepts an open trade listing.  
    /// If the trade is of type <c>FOR_CARD</c>, the buyer must provide their own card to exchange.
    /// </remarks>
    public class TradeExecuteRequest
    {
        /// <summary>
        /// The ID of the buyer's card being offered in exchange, if applicable.
        /// </summary>
        /// <remarks>
        /// Required only when executing a trade of type <c>FOR_CARD</c>.  
        /// Should be <c>null</c> for trades of type <c>FOR_PRICE</c>.
        /// </remarks>
        public Guid? BuyerCardId { get; set; }
    }
}
