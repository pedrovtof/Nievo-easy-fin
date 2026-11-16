using System.Text.Json.Serialization;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Interfaces.Response
{
    /// <summary>
    /// Response DTO exposing the authenticated user's profile details.
    /// </summary>
    public class GetUserDetailsResponse
    {
        /// <summary>
        /// Initializes a new instance of <see cref="GetUserDetailsResponse"/> from a <see cref="UserDetailsViews"/> projection.
        /// </summary>
        /// <param name="view">Database view containing user details.</param>
        public GetUserDetailsResponse(UserDetailsViews view)
        {
            Email = view.Email;
            Name = view.Name;
            Phone = view.Phone;
            UserStatus = view.UserStatus;
        }

        /// <summary>
        /// Email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        [JsonPropertyName("phone")]
        public int? Phone { get; set; }

        /// <summary>
        /// UserStatus
        /// </summary>
        [JsonPropertyName("status")]
        public string UserStatus { get; set; }
    }
}