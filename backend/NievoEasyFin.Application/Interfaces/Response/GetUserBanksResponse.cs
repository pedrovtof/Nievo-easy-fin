using System.Text.Json.Serialization;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Interfaces.Response
{
    /// <summary>
    /// Response contract
    /// </summary>
    public class GetUserBanksResponse
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="userBank"></param>
        public GetUserBanksResponse(UserBanksView userBank)
        {
            Name = userBank.Name;
            BankType = userBank.BankTypeName;
            NickName = userBank.NickName;
        }

        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// NickName
        /// </summary>
        [JsonPropertyName("nick_name")]
        public string NickName { get; set; }

        /// <summary>
        /// BankType
        /// </summary>
        [JsonPropertyName("bank_type")]
        public string BankType { get; set; }
    }
}
