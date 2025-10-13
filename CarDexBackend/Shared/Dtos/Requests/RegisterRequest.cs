namespace CarDexBackend.Shared.Dtos.Requests
{
    /// <summary>
    /// Represents a request to register a new user account.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// The desired username for the new account.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The password for the new account.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
