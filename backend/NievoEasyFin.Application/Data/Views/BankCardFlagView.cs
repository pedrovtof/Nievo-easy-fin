using System.Text.Json.Serialization;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Data.Views
{
    public class BankCardFlagView
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="entity"></param>
        public BankCardFlagView(BankCardFlagEntity entity)
        {
            Name = entity.Name;
            Description = entity.Description;
        }

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
        /// Records
        /// </summary>
        [JsonIgnore]
        public int Records { get; set; }
    }
}
