using System.Text.Json.Serialization;

namespace NievoEasyfin.Application.Interfaces.Request
{
    /// <summary>
    /// This class is a template for PostLoginUserAsync
    /// </summary>
    public class PostLoginUserRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
