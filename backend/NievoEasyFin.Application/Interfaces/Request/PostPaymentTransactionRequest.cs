using System.Numerics;
using System.Text.Json.Serialization;

namespace NievoEasyFin.Application.Interfaces.Request
{
    /// <summary>
    /// Request for create a user transaction
    /// </summary>
    public class PostPaymentTransactionRequest : ClaimRequestBase
    {
        /// <summary>
        /// Status of the transaction
        /// </summary>
        [JsonPropertyName("transaction_status")]
        public int TransactionStatus { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        [JsonPropertyName("category")]
        public int Category { get; set; }

        /// <summary>
        /// Pay date
        /// </summary>
        [JsonPropertyName("pay_date")]
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Card
        /// </summary>
        [JsonPropertyName("card")]
        public int Card { get; set; }

        /// <summary>
        /// Bank
        /// </summary>
        [JsonPropertyName("bank")]
        public int Bank { get; set; }

        /// <summary>
        /// Type of transaction
        /// </summary>
        [JsonPropertyName("transaction_type")]
        public int TransactionType { get; set; }

        /// <summary>
        /// Type of scheduler
        /// </summary>
        [JsonPropertyName("scheduler")]
        public int Scheduler { get; set; }

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
    }
}
