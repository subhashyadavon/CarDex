namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents the response returned when a user opens a pack.
    /// </summary>
    /// <remarks>
    /// This DTO includes both the pack information and the list of cards obtained upon opening it.  
    /// It is typically returned after a successful <c>POST /packs/{packId}/open</c> request.
    /// </remarks>
    public class PackOpenResponse
    {
        /// <summary>
        /// The list of cards generated from opening the pack.
        /// </summary>
        /// <remarks>
        /// Each card includes detailed information such as its grade, value, and associated collection.  
        /// The number and rarity of cards depend on the pack’s collection type and rarity system.
        /// </remarks>
        public IEnumerable<CardDetailedResponse> Cards { get; set; } = new List<CardDetailedResponse>();

        /// <summary>
        /// The pack that was opened.
        /// </summary>
        /// <remarks>
        /// Contains identifying information such as pack ID, collection details, and purchase date.  
        /// After opening, this pack will be marked as consumed or opened in the system.
        /// </remarks>
        public PackResponse Pack { get; set; } = new();
    }
}
