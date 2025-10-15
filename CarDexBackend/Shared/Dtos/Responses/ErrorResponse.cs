namespace CarDexBackend.Shared.Dtos.Responses
{
    /// <summary>
    /// Represents a standardized error response returned by the API.
    /// </summary>
    /// <remarks>
    /// This model is used when an operation fails due to invalid input, missing data,
    /// or unauthorized access.  
    /// It provides a human-readable message describing the issue.
    /// </remarks>
    public class ErrorResponse
    {
        /// <summary>
        /// A descriptive message explaining the reason for the error.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
