namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents detailed information about a specific trade.
    /// </summary>
    /// <remarks>
    /// Extends <see cref="TradeResponse"/> by including full card details for both the offered card and, if applicable,  
    /// the desired card. This response is typically returned from endpoints such as <c>GET /trades/{tradeId}</c>.
    /// </remarks>
    public class TradeDetailedResponse : TradeResponse
    {
        /// <summary>
        /// The detailed information of the card being offered in this trade.
        /// </summary>
        /// <remarks>
        /// Includes metadata such as card grade, value, and collection.  
        /// May be <c>null</c> if the card details are not included in the response context.
        /// </remarks>
        public CardDetailedResponse? Card { get; set; }

        /// <summary>
        /// The detailed information of the card desired in this trade.
        /// </summary>
        /// <remarks>
        /// Only applicable for <c>FOR_CARD</c> trades.  
        /// May be <c>null</c> for <c>FOR_PRICE</c> trades or if the desired card data is unavailable.
        /// </remarks>
        public CardDetailedResponse? WantCard { get; set; }
    }
}
