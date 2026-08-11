using Microsoft.AspNetCore.Mvc;

namespace NievoEasyFin.Application.Interfaces.Request
{
    /// <summary>
    /// Request template for get bank card
    /// </summary>
    public class GetBankCardRequest : PaginationRequestBase
    {
        /// <summary>
        /// Email
        /// </summary>
        private string Email;

        /// <summary>
        /// Bank Id
        /// </summary>
        [FromQuery(Name = "bank")]
        public int? BankId { get; set; }

        /// <summary>
        /// CardType
        /// </summary>
        [FromQuery(Name = "card_type")]
        public int? CardType { get; set; }

        /// <summary>
        /// Getter email
        /// </summary>
        /// <returns></returns>
        public string GetEmail()
        {
            return Email;
        }

        /// <summary>
        /// Setter email
        /// </summary>
        /// <param name="email"></param>
        public void SetEmail(string email)
        {
            Email = email;
        }
    }
}
