using System.Text.Json.Serialization;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Interfaces.Response
{
    public class GetPaymentTransactionTypeResponse
    {
        public GetPaymentTransactionTypeResponse(TransactionTypeView view)
        {
            Id = view.Id;
            Name = view.Name;
            Description = view.Description;
            Active = view.Active;
        }

        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

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
        /// Active
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }
}