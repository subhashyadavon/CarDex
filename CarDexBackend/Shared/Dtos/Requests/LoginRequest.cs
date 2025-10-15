namespace CarDexBackend.Shared.Dtos.Requests
{
    /// <summary>
    /// Represents a request to authenticate a user using their credentials.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// The username of the user attempting to log in.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The password associated with the user's account.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
