using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Interfaces.Response
{
    public class GetBanksResponse
    {
        public GetBanksResponse(BanksViews view)
        {
            Name = view.Name;
            BankType = view.BankType;
            BankTypeName = view.BankTypeName;
            Description = view.Description;
        }

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
