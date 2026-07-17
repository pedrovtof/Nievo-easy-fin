using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request
{
    /// <summary>
    /// This class is a template for PostAccountsBanks
    /// </summary>
    public class PostAccountsBanksRequest
    {
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// BankType
        /// </summary>
        [JsonPropertyName("bank_type")]
        public int BankType { get; set; }
    }
}
