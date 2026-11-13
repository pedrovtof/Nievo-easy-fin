using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Models
{
    public class TransactionModel : TransactionEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica _CoreReplicaNodeDatabase;

        public TransactionModel(CoreOrigin coreMainNodeDatabase, CoreReplica coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        ///  Search for list of payment transactions
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <param name="userBankCard"></param>
        /// <param name="scheduler"></param>
        /// <param name="transactionStatus"></param>
        /// <param name="transactionType"></param>
        /// <param name="category"></param>
        /// <param name="payDateStart"></param>
        /// <param name="payDateEnd"></param>
        /// <param name="situationId"></param>
        /// <returns></returns>
        public async Task<(List<TransactionView>, int)> GetPaymentTransactionAsync(
            int page,
            int pageSize,
            int userId,
            int? userBankCard,
            bool? scheduler,
            int? transactionStatus,
            int? transactionType,
            int? category,
            DateTime? payDateStart,
            DateTime? payDateEnd,
            int? situationId
        )
        {
            List<TransactionView> transactions = new();
            StringBuilder query = new();
            DynamicParameters parameters = new();

            query.Append(@"
                select 
                    t.id as Id,
                    t.name as Name,
                    t.description as Description,
                    t.amount as Amount,
                    st.cron_tab as CronTab,
                    ts.name as Status,
                    tt.name as Type,
                    c.name as Category,
                    t.pay_date as PayDate,
                    ubc.name as UserCardName,
                    ub.nick_name as UserBankNickName,
                    b.name as BankName,
                    tst.name as Situation,
                    cast(t.created_at as date) CreatedAt,
                    count(*) over() as Records
                from payment.""transaction"" t 
                    left join payment.scheduler_type st 
                        on t.scheduler_id = st.id
                    left join payment.transaction_status ts 
                        on t.transaction_status_id = ts.id 
                    left join payment.transaction_type tt 
                        on t.transaction_type_id  = tt.id 
                    left join payment.transaction_situation tst 
                        on t.transaction_situation_id  = tst.id 
                    left join goals.category c 
                        on t.categorie_id = c.id
                    left join accounts.user_bank_card ubc 
                        on t.card_id = ubc.id
                    left join accounts.user_bank ub 
                        on t.bank_id = ub.id 
                    left join accounts.bank b 
                        on ub.bank_id = b.id 
            ");

            query.Append(@"
                where t.user_id = @userId
            ");

            parameters.Add("userId", userId);

            if (scheduler == true)
                query.Append(@"
                    and t.scheduler_id is not null
                ");

            if (scheduler == false)
                query.Append(@"
                    and t.scheduler_id is null
                ");

            if (transactionStatus != null)
            {
                query.Append(@"
                    and ts.id = @transactionStatus
                ");
                parameters.Add("transactionStatus", transactionStatus);
            }

            if (situationId != null)
            {
                query.Append(@"
                    and tst.id = @transactionSituation
                ");
                parameters.Add("transactionSituation", situationId);
            }

            if (transactionType != null)
            {
                query.Append(@"
                    and tt.id = @transactionType
                ");
                parameters.Add("transactionType", transactionType);
            }

            if (userBankCard != null)
            {
                query.Append(@"
                    and ubc.id = @userBankCard
                ");
                parameters.Add("userBankCard", userBankCard);
            }

            if (category != null)
            {
                query.Append(@"
                    and c.id = @category
                ");
                parameters.Add("category", category);
            }

            if (payDateStart != null)
            {
                query.Append(@"
                    and t.pay_date >= @payDateStart
                ");
                parameters.Add("payDateStart", payDateStart);
            }

            if (payDateEnd != null)
            {
                query.Append(@"
                    and t.pay_date <= @payDateEnd
                ");
                parameters.Add("payDateEnd", payDateEnd);
            }

            query.Append(@"
                limit @limit
                offset @offset
            ");
            parameters.Add("limit", pageSize);
            parameters.Add("offset", (page - 1) * pageSize);

            var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();

            transactions.AddRange(
                await connection.QueryAsync<TransactionView>(query.ToString(), parameters)
            );

            int records = transactions.Any() ? transactions.FirstOrDefault().Records : 0;

            return (transactions, records);
        }

        /// <summary>
        /// Create new transaction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="user_id"></param>
        /// <param name="transaction_status"></param>
        /// <param name="transaction_type"></param>
        /// <param name="category"></param>
        /// <param name="pay_date"></param>
        /// <param name="amount"></param>
        /// <param name="card"></param>
        /// <param name="bank"></param>
        /// <param name="scheduler"></param>
        /// <param name="transaction_situation"></param>
        /// <returns></returns>
        public async Task<TransactionEntity> CreatePaymentTransaction(
            string name,
            string description,
            int user_id,
            int transaction_status,
            int transaction_type,
            int category,
            DateTime pay_date,
            Decimal amount,
            int? card,
            int bank,
            int? scheduler,
            int transaction_situation
        )
        {
            pay_date = new DateTime(pay_date.Ticks);

            TransactionEntity entity = new()
            {
                Name = name,
                Description = description,
                UserId = user_id,
                TransactionStatusId = transaction_status,
                TransactionTypeId = transaction_type,
                CategorieId = category,
                PayDate = pay_date,
                Amount = amount,
                CardId = card,
                BankId = bank,
                SchedulerId = scheduler,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                TransactionSituationId = transaction_situation
            };

            await _CoreMainNodeDatabase.Transaction.AddAsync(entity);
            await _CoreMainNodeDatabase.SaveChangesAsync();

            return entity;
        }
    }
}