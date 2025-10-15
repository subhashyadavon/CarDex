namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a finalized trade between two users.
    /// </summary>
    /// <remarks>
    /// This DTO contains all key details about a completed trade,  
    /// including both participants, traded items, trade type, price, and execution timestamp.  
    /// Returned primarily by endpoints such as <c>GET /trades/history</c> and <c>GET /trades/history/{tradeId}</c>.
    /// </remarks>
    public class CompletedTradeResponse
    {
        /// <summary>
        /// The unique identifier for this completed trade.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The type of trade that was completed.
        /// </summary>
        /// <remarks>
        /// Can be <c>FOR_CARD</c> or <c>FOR_PRICE</c>.
        /// </remarks>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// The unique ID of the seller user.
        /// </summary>
        public Guid SellerUserId { get; set; }

        /// <summary>
        /// The username of the seller.
        /// </summary>
        public string SellerUsername { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the card sold or traded by the seller.
        /// </summary>
        public Guid SellerCardId { get; set; }

        /// <summary>
        /// The unique ID of the buyer user.
        /// </summary>
        public Guid BuyerUserId { get; set; }

        /// <summary>
        /// The username of the buyer.
        /// </summary>
        public string BuyerUsername { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the card offered by the buyer, if applicable.
        /// </summary>
        /// <remarks>
        /// This field is <c>null</c> for <c>FOR_PRICE</c> trades and populated for <c>FOR_CARD</c> trades.
        /// </remarks>
        public Guid? BuyerCardId { get; set; }

        /// <summary>
        /// The currency amount paid by the buyer in this trade.
        /// </summary>
        /// <remarks>
        /// For <c>FOR_CARD</c> trades, this value is typically <c>0</c>.
        /// </remarks>
        public int Price { get; set; }

        /// <summary>
        /// The timestamp indicating when the trade was completed.
        /// </summary>
        public DateTime ExecutedDate { get; set; }
    }
}
