namespace CarDexBackend.Shared.Dtos.Requests
{
    /// <summary>
    /// Represents a request to create a new trade listing.
    /// </summary>
    /// <remarks>
    /// A trade can either be listed for a specific card (<c>FOR_CARD</c>) or for a set currency price (<c>FOR_PRICE</c>).
    /// </remarks>
    public class TradeCreateRequest
    {
        /// <summary>
        /// The type of trade being created.
        /// </summary>
        /// <remarks>
        /// Must be either <c>FOR_CARD</c> (trading for another card) or <c>FOR_PRICE</c> (selling for currency).
        /// </remarks>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// The unique identifier of the card being offered in the trade.
        /// </summary>
        public Guid CardId { get; set; }

        /// <summary>
        /// The asking price for the trade, if the type is <c>FOR_PRICE</c>.
        /// </summary>
        /// <remarks>
        /// This field should be <c>null</c> if the trade is of type <c>FOR_CARD</c>.
        /// </remarks>
        public int? Price { get; set; }

        /// <summary>
        /// The ID of the specific card being requested in exchange, if the trade is <c>FOR_CARD</c>.
        /// </summary>
        /// <remarks>
        /// This field should be <c>null</c> if the trade is of type <c>FOR_PRICE</c>.
        /// </remarks>
        public Guid? WantCardId { get; set; }
    }
}
