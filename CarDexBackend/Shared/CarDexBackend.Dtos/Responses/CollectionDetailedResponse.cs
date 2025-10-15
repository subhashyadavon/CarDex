namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a detailed view of a collection, extending <see cref="CollectionResponse"/>.
    /// </summary>
    /// <remarks>
    /// This DTO is used when returning complete information about a specific collection,
    /// including all the cards that belong to it.
    /// </remarks>
    public class CollectionDetailedResponse : CollectionResponse
    {
        /// <summary>
        /// The list of cards that are part of this collection.
        /// </summary>
        /// <remarks>
        /// Each item provides summary information for a card.
        /// Use the <see cref="CardDetailedResponse"/> endpoint to retrieve full card details.
        /// </remarks>
        public IEnumerable<CardResponse> Cards { get; set; } = new List<CardResponse>();
    }
}
