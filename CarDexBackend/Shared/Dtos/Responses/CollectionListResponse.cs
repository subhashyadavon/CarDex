namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a response containing a list of available collections.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used for endpoints that return multiple collections,
    /// such as the marketplace or collection browsing APIs.
    /// </remarks>
    public class CollectionListResponse
    {
        /// <summary>
        /// A list of collections matching the query or available in the catalog.
        /// </summary>
        /// <remarks>
        /// Each item contains summary information about a collection.  
        /// Use <see cref="CollectionDetailedResponse"/> to retrieve full details for a specific collection.
        /// </remarks>
        public IEnumerable<CollectionResponse> Collections { get; set; } = new List<CollectionResponse>();

        /// <summary>
        /// The total number of collections available that match the query.
        /// </summary>
        public int Total { get; set; }
    }
}
