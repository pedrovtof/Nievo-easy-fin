using Microsoft.AspNetCore.Mvc;

namespace NievoEasyFin.Application.Interfaces.Request
{
    public class GetUserCardRequest : PaginationClaimRequestBase
    {
        /// <summary>
        /// Bank id
        /// </summary>
        [FromQuery(Name = "bank_id")]
        public int? BankId { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        [FromQuery(Name = "active")]
        public bool Active { get; set; } = true;
    }
}
