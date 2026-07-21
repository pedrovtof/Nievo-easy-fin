using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Response
{
    /// <summary>
    /// Class for return pagination base
    /// </summary>
    public class ResponsePaginationBase<T>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="page">int</param>
        /// <param name="pageSize">int</param>
        /// <param name="records">int</param>
        /// <param name="view">T</param>
        public ResponsePaginationBase(int page, int pageSize, int records, List<T> view)
        {
            Page = page;
            PageSize = pageSize;
            Records = records;
            Items = view;
        }

        /// <summary>
        /// Page
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }

        /// <summary>
        /// Records number
        /// </summary>
        [JsonPropertyName("records")]
        public int Records { get; set; }

        /// <summary>
        /// List of items
        /// </summary>
        [JsonPropertyName("items")]
        public List<T> Items { get; set; }
    }
}
