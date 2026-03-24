using System.Text.Json.Serialization;

namespace NievoEasyfin.Application.Interfaces.Request
{
    public class PostUserRequest
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}