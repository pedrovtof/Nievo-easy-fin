using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Models
{
    public class TransactionTypeModel : TransactionTypeEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica _CoreReplicaNodeDatabase;

        public TransactionTypeModel(CoreOrigin coreMainNodeDatabase, CoreReplica coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// Method to search for transaction Type
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(List<TransactionTypeView>, int)> GetTransactionTypeAsync(int page, int pageSize)
        {
            List<TransactionTypeView> transactionType = new();
            StringBuilder query = new();
            DynamicParameters parameters = new();

            query.Append("""
                select 
                    tp.id as Id,
                    tp.name as Name, 
                    tp.active as Active,
                    tp.description as Description,
                    count(*) over() as Records
                from payment.transaction_type tp 
                where tp.active is true
                order by tp.id
                limit @limit
                offset @offset
            """);

            parameters.Add("limit", pageSize);
            parameters.Add("offset", (page - 1) * pageSize);

            var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();

            transactionType.AddRange(
                await connection.QueryAsync<TransactionTypeView>(query.ToString(), parameters)
            );

            int records = transactionType.Any() ? transactionType.FirstOrDefault().Records : 0;

            return (transactionType, records);
        }
    }
}
