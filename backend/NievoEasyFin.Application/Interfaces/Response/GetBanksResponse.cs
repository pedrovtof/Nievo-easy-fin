using System.Text.Json.Serialization;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Interfaces.Response
{
    /// <summary>
    /// Class for response in get banks
    /// </summary>
    public class GetBanksResponse
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="view">BanksViews</param>
        public GetBanksResponse(BanksViews view)
        {
            BankId = view.BankId;
            Name = view.Name;
            BankType = view.BankType;
            BankTypeName = view.BankTypeName;
            Description = view.Description;
        }

        /// <summary>
        /// Bank id
        /// </summary>
        [JsonPropertyName("bank_id")]
        public int BankId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Bank type
        /// </summary> 
        [JsonPropertyName("bank_type")]
        public int BankType { get; set; }

        /// <summary>
        /// Bank type name
        /// </summary> 
        [JsonPropertyName("bank_type_name")]
        public string BankTypeName { get; set; }

        /// <summary>
        /// Bank description
        /// </summary> 
        [JsonPropertyName("bank_description")]
        public string Description { get; set; }
    }
}
