using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;
using Superpower.Parsers;

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
        /// <returns></returns>
        public async Task<TransactionEntity> CreatePaymentTransaction(
            string name,
            string description,
            int user_id,
            int transaction_status,
            int transaction_type,
            int category,
            DateTime pay_date,
            long amount,
            int? card,
            int bank,
            int? scheduler
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
                UpdatedAt = DateTime.Now
            };

            await _CoreMainNodeDatabase.Transaction.AddAsync(entity);
            await _CoreMainNodeDatabase.SaveChangesAsync();

            return entity;
        }
    }
}