namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a single card owned by a user.
    /// </summary>
    /// <remarks>
    /// Each card corresponds to a collectible vehicle within a specific collection.  
    /// This DTO is typically returned by <c>GET /users/{userId}/cards</c> and  
    /// may include filtering options such as <c>collectionId</c> or <c>grade</c>.
    /// </remarks>
    public class UserCardResponse
    {
        /// <summary>
        /// The unique identifier of the card.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The identifier of the vehicle represented by this card.
        /// </summary>
        public Guid VehicleId { get; set; }

        /// <summary>
        /// The identifier of the collection this card belongs to.
        /// </summary>
        public Guid CollectionId { get; set; }

        /// <summary>
        /// The grade or rarity level of the card.
        /// </summary>
        public string Grade { get; set; } = string.Empty;

        /// <summary>
        /// The in-game market value of the card.
        /// </summary>
        public int Value { get; set; }
    }

    /// <summary>
    /// Represents a paginated list of cards owned by a user.
    /// </summary>
    /// <remarks>
    /// This DTO includes pagination metadata and is used in  
    /// responses from <c>GET /users/{userId}/cards</c>.
    /// </remarks>
    public class UserCardListResponse
    {
        /// <summary>
        /// The list of cards owned by the user.
        /// </summary>
        public IEnumerable<UserCardResponse> Cards { get; set; } = new List<UserCardResponse>();

        /// <summary>
        /// The total number of cards that match the query.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// The maximum number of cards returned in this page.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// The number of cards skipped before the current page.
        /// </summary>
        public int Offset { get; set; }
    }
}
