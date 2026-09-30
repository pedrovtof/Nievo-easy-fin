using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Data.Views
{
    public class UserCategoryView
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// active
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        /// <summary>
        /// ParentCategoryName
        /// </summary>
        [JsonPropertyName("parent_category_name")]
        public string? ParentCategoryName { get; set; }

        /// <summary>
        /// ParentCategoryDescription
        /// </summary>
        [JsonPropertyName("parent_category_description")]
        public string? ParentCategoryDescription { get; set; }

        /// <summary>
        /// ParentCategoryActive
        /// </summary>
        [JsonPropertyName("parent_category_active")]
        public bool? ParentCategoryActive { get; set; }

        /// <summary>
        /// GoalName
        /// </summary>
        [JsonPropertyName("goal_name")]
        public string GoalName { get; set; }

        /// <summary>
        /// GoalDescription
        /// </summary>
        [JsonPropertyName("goal_description")]
        public string GoalDescription { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// UpdatedAt
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Records
        /// </summary>
        [JsonIgnore]
        public int Records { get; set; }
    }
}
