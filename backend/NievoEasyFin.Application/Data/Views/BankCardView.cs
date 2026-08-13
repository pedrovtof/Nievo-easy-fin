using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Data.Views
{
    /// <summary>
    /// View class for bankCardEntity
    /// </summary>
    public class BankCardView
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Bank
        /// </summary>
        [JsonPropertyName("bank")]
        public string Bank { get; set; }

        /// <summary>
        /// CardType
        /// </summary>
        [JsonPropertyName("card_type")]
        public string CardType { get; set; }

        /// <summary>
        /// Card
        /// </summary>
        [JsonPropertyName("card_name")]
        public string CardName { get; set; }

        /// <summary>
        /// Records
        /// </summary>
        [JsonIgnore]
        public int Records { get; set; }
    }
}
