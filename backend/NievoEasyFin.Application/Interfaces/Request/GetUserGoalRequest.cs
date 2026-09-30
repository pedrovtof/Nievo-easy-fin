using Microsoft.AspNetCore.Mvc;

namespace NievoEasyFin.Application.Interfaces.Request
{
    public class GetUserGoalRequest : PaginationClaimRequestBase
    {
        /// <summary>
        /// active
        /// </summary>
        [FromQuery(Name = "active")]
        public bool Active { get; set; } = true;
    }
}
