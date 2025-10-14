namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a paginated list of card summaries returned by the API.
    /// </summary>
    /// <remarks>
    /// This response is typically used for endpoints that return multiple cards,
    /// such as browsing or searching through available cards.
    /// </remarks>
    public class CardListResponse
    {
        /// <summary>
        /// A collection of cards matching the query or filter criteria.
        /// </summary>
        public IEnumerable<CardResponse> Cards { get; set; } = new List<CardResponse>();

        /// <summary>
        /// The total number of cards available that match the query, before pagination.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// The maximum number of cards returned in this response (page size).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// The number of cards skipped before this page (pagination offset).
        /// </summary>
        public int Offset { get; set; }
    }
}
