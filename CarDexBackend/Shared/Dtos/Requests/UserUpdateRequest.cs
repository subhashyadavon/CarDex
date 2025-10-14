namespace CarDexBackend.Shared.Dtos.Requests
{
    /// <summary>
    /// Represents a request to update user profile information.
    /// </summary>
    /// <remarks>
    /// Both fields are optional.  
    /// This request can be used to update either the username, password, or both.
    /// </remarks>
    public class UserUpdateRequest
    {
        /// <summary>
        /// The new username for the user account.
        /// </summary>
        /// <remarks>
        /// Must be unique within the system.  
        /// Leave <c>null</c> or empty to keep the current username.
        /// </remarks>
        public string? Username { get; set; }

        /// <summary>
        /// The new password for the user account.
        /// </summary>
        /// <remarks>
        /// Leave <c>null</c> or empty to keep the current password.  
        /// Should meet the same security requirements as registration passwords.
        /// </remarks>
        public string? Password { get; set; }
    }
}
