using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace NievoEasyFin.Application.Interfaces.Request
{
    /// <summary>
    /// Classe for pagination request
    /// </summary>
    public class PaginationRequestBase
    {
        /// <summary>
        /// Page
        /// </summary>
        [FromQuery(Name = "page")]
        public int Page { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        [FromQuery(Name = "page_size")]
        public int PageSize { get; set; }
    }
}
