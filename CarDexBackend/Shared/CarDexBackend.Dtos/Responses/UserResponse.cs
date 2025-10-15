namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a user account within the system.
    /// </summary>
    /// <remarks>
    /// This DTO contains general user information, including identifiers, username,  
    /// in-game currency, and account creation and update timestamps.  
    /// Returned by authentication endpoints (e.g. <c>/auth/register</c>, <c>/auth/login</c>)  
    /// and user profile endpoints (e.g. <c>/users/{userId}</c>).
    /// </remarks>
    public class UserResponse
    {
        /// <summary>
        /// The unique identifier of the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The user's chosen username.
        /// </summary>
        /// <remarks>
        /// Must be unique within the system.
        /// </remarks>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The amount of in-game currency currently held by the user.
        /// </summary>
        public int Currency { get; set; }

        /// <summary>
        /// The timestamp when the user account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The timestamp when the user profile was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
