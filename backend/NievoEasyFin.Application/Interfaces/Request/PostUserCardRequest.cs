
using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request
{
    public class PostUserCardRequest : ClaimRequestBase
    {
        /// <summary>
        /// Bank id
        /// </summary>
        [JsonPropertyName("bank_id")]
        public int BankId { get; set; }

        /// <summary>
        /// Card user name
        /// </summary>
        [JsonPropertyName("nickname")]
        public string CardUserName { get; set; }

        /// <summary>
        /// Card Id
        /// </summary>
        [JsonPropertyName("card_id")]
        public int CardId { get; set; }

        /// <summary>
        /// Expire at
        /// </summary>
        [JsonPropertyName("expire_at")]
        public DateTime ExpireAt { get; set; }

    }
}