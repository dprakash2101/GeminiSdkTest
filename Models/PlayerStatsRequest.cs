namespace GeminiSdkTest.Models
{
    //// <summary>
    /// Represents the request body for generating structured cricket player statistics.
    /// </summary>
    public class PlayerStatsRequest
    {
        /// <summary>
        /// The prompt to generate cricket stats.
        /// </summary>
        public string Prompt { get; set; } = string.Empty;
    }

}
