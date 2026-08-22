using Microsoft.AspNetCore.Mvc;

namespace NievoEasyFin.Application.Interfaces.Request
{
    /// <summary>
    /// Request template for get bank card
    /// </summary>
    public class GetBankCardRequest : PaginationClaimRequestBase
    {
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
        /// Flag name
        /// </summary>
        [FromQuery(Name = "flag")]
        public string? Flag { get; set; }
    }
}
