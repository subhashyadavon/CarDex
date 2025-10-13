namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a detailed view of a pack, extending basic pack information.
    /// </summary>
    /// <remarks>
    /// This DTO is used when returning a single pack with additional details such as 
    /// preview cards and an estimated total value.  
    /// Typically returned when viewing a specific pack or after a purchase.
    /// </remarks>
    public class PackDetailedResponse : PackResponse
    {
        /// <summary>
        /// A preview of the cards that may be found in this pack.
        /// </summary>
        /// <remarks>
        /// This is not a guarantee of the final contents but provides insight 
        /// into potential pulls within the collection.
        /// </remarks>
        public IEnumerable<CardResponse> PreviewCards { get; set; } = new List<CardResponse>();

        /// <summary>
        /// The estimated market value of this pack before it is opened.
        /// </summary>
        /// <remarks>
        /// This value is an approximation based on average card values for the associated collection.
        /// </remarks>
        public int EstimatedValue { get; set; }
    }
}
