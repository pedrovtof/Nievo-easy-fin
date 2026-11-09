using Microsoft.AspNetCore.Mvc;

namespace NievoEasyFin.Application.Interfaces.Request
{
    public class GetPaymentTransactionRequest : PaginationClaimRequestBase
    {
        /// <summary>
        /// UserBankCard
        /// </summary>
        [FromQuery(Name = "user_bank_card")]
        public int? UserBankCard { get; set; }

        /// <summary>
        /// SchedulerExists
        /// </summary>
        [FromQuery(Name = "scheduler_exists")]
        public bool? SchedulerExists { get; set; }

        /// <summary>
        /// TransactionStatusId
        /// </summary>
        [FromQuery(Name = "transaction_status_id")]
        public int? TransactionStatusId { get; set; }

        /// <summary>
        /// TransactionTypeId
        /// </summary>
        [FromQuery(Name = "transaction_type_id")]
        public int? TransactionTypeId { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        [FromQuery(Name = "category")]
        public int? Category { get; set; }

        /// <summary>
        /// PayDateStart
        /// </summary>
        [FromQuery(Name = "pay_date_start")]
        public DateTime? PayDateStart { get; set; }

        /// <summary>
        /// PayDateEnd
        /// </summary>
        [FromQuery(Name = "pay_date_end")]
        public DateTime? PayDateEnd { get; set; }
    }
}
