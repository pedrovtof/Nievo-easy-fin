using System.Text.Json.Serialization;

namespace NievoEasyfin.Application.Interfaces.Request
{
    /// <summary>
    /// This class is a template for PostLoginUserSsoAsync
    /// </summary>
    public class PostLogiPostLoginUserSsoRequest
    {
        /// <summary>
        /// Name of the provider (journey.sso_provider)
        /// </summary>
        [JsonPropertyName("provider_name")]
        public string Provider { get; set; }

        /// <summary>
        /// User Id from provider
        /// </summary>
        [JsonPropertyName("provider_access_token")]
        public string ProviderAccessToken { get; set; }
    }
}
