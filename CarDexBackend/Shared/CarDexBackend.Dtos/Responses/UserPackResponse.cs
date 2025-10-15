namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents an unopened pack owned by a user.
    /// </summary>
    /// <remarks>
    /// Each pack corresponds to a collectible pack purchased from a specific collection.  
    /// Returned by the <c>GET /users/{userId}/packs</c> endpoint.
    /// </remarks>
    public class UserPackResponse
    {
        /// <summary>
        /// The unique identifier of the pack.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The identifier of the collection this pack belongs to.
        /// </summary>
        public Guid CollectionId { get; set; }

        /// <summary>
        /// The estimated value of the pack in in-game currency.
        /// </summary>
        /// <remarks>
        /// This may represent the combined worth of the cards expected from this pack.
        /// </remarks>
        public int Value { get; set; }
    }

    /// <summary>
    /// Represents a list of unopened packs owned by a user.
    /// </summary>
    /// <remarks>
    /// This DTO is returned by the <c>GET /users/{userId}/packs</c> endpoint  
    /// and provides a summary of all packs currently available in the user's inventory.
    /// </remarks>
    public class UserPackListResponse
    {
        /// <summary>
        /// The list of packs owned by the user.
        /// </summary>
        public IEnumerable<UserPackResponse> Packs { get; set; } = new List<UserPackResponse>();

        /// <summary>
        /// The total number of packs in the user's inventory.
        /// </summary>
        public int Total { get; set; }
    }
}
