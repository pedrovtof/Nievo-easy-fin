using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace NievoEasyfin.Application.Interfaces.Response
{
    public class ResponseProvider
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        [JsonPropertyName("sub")]
        public string? Sub { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        [JsonPropertyName("given_name")]
        public string? GivenName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        [JsonPropertyName("family_name")]
        public string? FamilyName { get; set; }

        /// <summary>
        /// Url for picture
        /// </summary>
        [JsonPropertyName("picture")]
        public string? Picture { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Email is verified
        /// </summary>
        [JsonPropertyName("email_verified")]
        public bool? EmailVerified { get; set; }

        /// <summary>
        /// Has some error the request
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        public void WithError(string error)
        {
            Error = error;
        }
    }
}