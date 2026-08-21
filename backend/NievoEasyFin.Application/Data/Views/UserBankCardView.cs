using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Views
{
    public class UserBankCardView
    {
        /// <summary>
        /// User Bank card
        /// </summary>
        [JsonPropertyName("user_bank_card_id")]
        public int UserBankCardId { get; set; }

        /// <summary>
        /// User bank card name
        /// </summary>
        [JsonPropertyName("user_bank_card_nickname")]
        public string UserBankCardName { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        /// <summary>
        /// Expired at
        /// </summary>
        [JsonPropertyName("expired_at")]
        public DateTime ExpiredAt { get; set; }

        /// <summary>
        /// Bank name
        /// </summary>
        [JsonPropertyName("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        /// Bank card name
        /// </summary>
        [JsonPropertyName("bank_card_name")]
        public string BankCardName { get; set; }

        /// <summary>
        /// Bank card type
        /// </summary>
        [JsonPropertyName("bank_card_type")]
        public string BankCardType { get; set; }

        /// <summary>
        /// Records
        /// </summary>
        [JsonIgnore]
        public int Records { get; set; }
    }
}
