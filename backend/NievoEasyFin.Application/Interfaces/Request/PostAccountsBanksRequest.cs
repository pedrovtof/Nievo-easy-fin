using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request
{
    /// <summary>
    /// This class is a template for PostAccountsBanks
    /// </summary>
    public class PostAccountsBanksRequest
    {
        private string UserMail;

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

        /// <summary>
        /// Set the user mail from token
        /// </summary>
        /// <param name="x">User mail</param>
        public void SetUserMail(string x) => UserMail = x;

        /// <summary>
        /// Get the user mail
        /// </summary>
        /// <returns>User mail string</returns>
        public string GetUserMail() => UserMail;
    }
}