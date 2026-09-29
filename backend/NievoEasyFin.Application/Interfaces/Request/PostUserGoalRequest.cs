using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request
{
    public class PostUserGoalRequest : ClaimRequestBase
    {
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// ExpireAt
        /// </summary>
        [JsonPropertyName("expire_at")]
        public DateTime ExpireAt { get; set; }

        /// <summary>
        /// IsPercent
        /// </summary>
        [JsonPropertyName("is_percent")]
        public bool IsPercent { get; set; }
    }
}
