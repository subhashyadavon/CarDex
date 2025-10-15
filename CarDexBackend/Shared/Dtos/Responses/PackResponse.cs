namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a summary view of a pack owned or purchased by a user.
    /// </summary>
    /// <remarks>
    /// This DTO provides basic pack information such as its collection association, purchase date,
    /// and whether the pack has been opened.  
    /// Use <see cref="PackDetailedResponse"/> for a full breakdown of the pack contents.
    /// </remarks>
    public class PackResponse
    {
        /// <summary>
        /// The unique identifier for the pack.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique identifier of the collection this pack belongs to.
        /// </summary>
        public Guid CollectionId { get; set; }

        /// <summary>
        /// The name of the collection this pack belongs to.
        /// </summary>
        public string CollectionName { get; set; } = string.Empty;

        /// <summary>
        /// The UTC timestamp indicating when the pack was purchased.
        /// </summary>
        public DateTime PurchasedAt { get; set; }

        /// <summary>
        /// Indicates whether the pack has already been opened.
        /// </summary>
        public bool IsOpened { get; set; }
    }
}
