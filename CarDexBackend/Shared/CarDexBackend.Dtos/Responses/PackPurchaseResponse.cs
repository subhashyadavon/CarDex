namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents the result of a successful pack purchase.
    /// </summary>
    /// <remarks>
    /// Returned when a user buys a pack from a collection.  
    /// Contains both the purchased pack details and the user's remaining currency balance.
    /// </remarks>
    public class PackPurchaseResponse
    {
        /// <summary>
        /// The pack that was successfully purchased.
        /// </summary>
        /// <remarks>
        /// Includes information such as the pack ID, collection name, and purchase date.  
        /// Use this object to track or display the newly acquired pack in the user's inventory.
        /// </remarks>
        public PackResponse Pack { get; set; } = new();

        /// <summary>
        /// The user’s remaining in-game currency after the purchase.
        /// </summary>
        public int UserCurrency { get; set; }
    }
}
