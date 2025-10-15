namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents the response returned after a successful user login.
    /// </summary>
    /// <remarks>
    /// This DTO contains the access token, its metadata, and user details.  
    /// Clients should store the <see cref="AccessToken"/> securely and include it in the 
    /// <c>Authorization</c> header for subsequent authenticated requests.
    /// </remarks>
    public class LoginResponse
    {
        /// <summary>
        /// The access token issued to the authenticated user.
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// The type of the token returned. Usually <c>"Bearer"</c>.
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// The number of seconds until the access token expires.
        /// </summary>
        public int ExpiresIn { get; set; } = 3600;

        /// <summary>
        /// Information about the authenticated user.
        /// </summary>
        public UserResponse User { get; set; } = new();
    }
}
