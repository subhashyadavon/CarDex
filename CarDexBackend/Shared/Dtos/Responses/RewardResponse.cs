namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a reward granted to a user.
    /// </summary>
    /// <remarks>
    /// Rewards are typically granted after completing trades, opening packs, or achieving milestones.  
    /// A reward can represent in-game currency, a pack, or a specific card.  
    /// This DTO may be returned by endpoints such as <c>GET /users/{userId}/rewards</c> or trade execution endpoints.
    /// </remarks>
    public class RewardResponse
    {
        /// <summary>
        /// The unique identifier of this reward.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the user who owns this reward.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The type of reward granted.
        /// </summary>
        /// <remarks>
        /// Valid types include:
        /// <list type="bullet">
        /// <item><description><c>PACK</c> – a new unopened pack.</description></item>
        /// <item><description><c>CURRENCY</c> – bonus or trade currency.</description></item>
        /// <item><description><c>CARD_FROM_TRADE</c> – a card obtained through trade.</description></item>
        /// <item><description><c>CURRENCY_FROM_TRADE</c> – funds earned from a trade.</description></item>
        /// </list>
        /// </remarks>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the related item (such as a pack or card), if applicable.
        /// </summary>
        /// <remarks>
        /// This property is <c>null</c> for currency rewards.
        /// </remarks>
        public Guid? ItemId { get; set; }

        /// <summary>
        /// The amount of currency associated with the reward.
        /// </summary>
        /// <remarks>
        /// Only applies to rewards of type <c>CURRENCY</c> or <c>CURRENCY_FROM_TRADE</c>.
        /// </remarks>
        public int? Amount { get; set; }

        /// <summary>
        /// The timestamp when the reward was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The timestamp when the reward was claimed by the user, if applicable.
        /// </summary>
        /// <remarks>
        /// Will be <c>null</c> for unclaimed rewards.
        /// </remarks>
        public DateTime? ClaimedAt { get; set; }
    }
}
