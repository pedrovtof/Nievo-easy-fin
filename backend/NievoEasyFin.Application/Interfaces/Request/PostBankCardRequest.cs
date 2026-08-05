using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request
{
    /// <summary>
    /// Template for create an bank card
    /// </summary>
    public class PostBankCardRequest
    {
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Bank Id
        /// </summary>
        [JsonPropertyName("bank_id")]
        public int BankId { get; set; }

        /// <summary>
        /// Card Type
        /// </summary>
        [JsonPropertyName("card_type")]
        public int CardType { get; set; }
    }
}