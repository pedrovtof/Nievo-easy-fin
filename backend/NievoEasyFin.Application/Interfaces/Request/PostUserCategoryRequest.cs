using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request
{
    public class PostUserCategoryRequest : ClaimRequestBase
    {
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Goal
        /// </summary>
        [JsonPropertyName("goal")]
        public int Goal { get; set; }

        /// <summary>
        /// Parent category
        /// </summary>
        [JsonPropertyName("parent_category")]
        public int? ParentCategory { get; set; }
    }
}
