using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Data.Views
{
    public class UserGoalView
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public bool Id { get; set; }

        /// <summary>
        /// IsPercent
        /// </summary>
        [JsonPropertyName("is_percent")]
        public bool IsPercent { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

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
        /// ExpireAt
        /// </summary>
        [JsonPropertyName("expire_at")]
        public DateTime ExpireAt { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// UpdatedAt
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Records
        /// </summary>
        [JsonIgnore]
        public int Records { get; set; }
    }
}
