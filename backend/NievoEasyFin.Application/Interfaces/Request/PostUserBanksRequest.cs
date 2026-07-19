using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request
{
    /// <summary>
    /// This class is a template for PostUserBank
    /// </summary>
    public class PostUserBanksRequest
    {
        /// <summary>
        /// Nickname of the bank
        /// </summary>
        [JsonPropertyName("nickname")]
        public string? NickName { get; set; }

        /// <summary>
        /// Bank identity
        /// </summary>
        [JsonPropertyName("bank_type")]
        public int BankType { get; set; }

        /// <summary>
        /// Bank name
        /// </summary>
        [JsonPropertyName("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        [JsonPropertyName("email")]
        private string Email { get; set; }

        /// <summary>
        /// Setter Email
        /// </summary>
        /// <param name="email">string</param>
        public void SetEmail(string email)
        {
            Email = email;
        }

        /// <summary>
        /// Getter Email
        /// </summary>
        /// <returns>Email</returns>
        public string GetEmail()
        {
            return Email;
        }
    }
}
