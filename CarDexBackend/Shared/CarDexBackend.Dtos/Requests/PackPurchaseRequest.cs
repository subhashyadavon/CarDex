namespace CarDexBackend.Shared.Dtos.Requests
{
    /// <summary>
    /// Represents a request to purchase a pack from a specific collection.
    /// </summary>
    public class PackPurchaseRequest
    {
        /// <summary>
        /// The unique identifier of the collection to purchase a pack from.
        /// </summary>
        public Guid CollectionId { get; set; }
    }
}
