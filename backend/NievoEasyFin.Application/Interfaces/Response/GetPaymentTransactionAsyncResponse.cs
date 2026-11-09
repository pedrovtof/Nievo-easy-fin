using System.Text.Json.Serialization;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;


namespace NievoEasyFin.Application.Interfaces.Response
{
    public class GetPaymentTransactionAsyncResponse
    {
        public GetPaymentTransactionAsyncResponse(TransactionView view)
        {
            Id = view.Id;
            Name = view.Name;
            Description = view.Description;
            PayDate = view.PayDate;
            Amount = view.Amount;
            CreatedAt = view.CreatedAt;
            CronTab = view.CronTab;
            Status = view.Status;
            Type = view.Type;
            Category = view.Category;
            UserCardName = view.UserCardName;
            UserBankNickName = view.UserBankNickName;
            BankName = view.BankName;
        }

        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

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
        /// Amount
        /// </summary>
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Cron Tab
        /// </summary>
        [JsonPropertyName("cron_tab")]
        public string CronTab { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; }

        /// <summary>
        /// PayDate
        /// </summary>
        [JsonPropertyName("pay_date")]
        public DateTime PayDate { get; set; }

        /// <summary>
        /// UserCardName
        /// </summary>
        [JsonPropertyName("user_card_name")]
        public string UserCardName { get; set; }

        /// <summary>
        /// UserBankNickName
        /// </summary>
        [JsonPropertyName("user_bank_nickname")]
        public string UserBankNickName { get; set; }

        /// <summary>
        /// BankName
        /// </summary>
        [JsonPropertyName("bank_name")]
        public string BankName { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
