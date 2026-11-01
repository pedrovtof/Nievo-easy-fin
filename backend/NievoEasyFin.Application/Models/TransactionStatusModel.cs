using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Models
{
    public class TransactionStatusModel : TransactionStatusEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica _CoreReplicaNodeDatabase;

        public TransactionStatusModel(CoreOrigin coreMainNodeDatabase, CoreReplica coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// Method to search for transaction status
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(List<TransactionStatusView>, int)> GetTransactionStatusAsync(int page, int pageSize)
        {
            List<TransactionStatusView> transactionStatus = new();
            StringBuilder query = new();
            DynamicParameters parameters = new();

            query.Append("""
                select 
                    ts.id as Id,
                    ts.name as Name, 
                    ts.active as Active,
                    ts.description as Description,
                    count(*) over() as Records
                from payment.transaction_status ts 
                where ts.active is true
                order by ts.id
                limit @limit
                offset @offset
            """);

            parameters.Add("limit", pageSize);
            parameters.Add("offset", (page - 1) * pageSize);

            var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();

            transactionStatus.AddRange(
                await connection.QueryAsync<TransactionStatusView>(query.ToString(), parameters)
            );

            int records = transactionStatus.Any() ? transactionStatus.FirstOrDefault().Records : 0;

            return (transactionStatus, records);
        }

    }
}