using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Models
{
    /// <summary>
    /// Model responsible for data access operations on transaction situations.
    /// </summary>
    public class TransactionSituationModel : TransactionSituationEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica _CoreReplicaNodeDatabase;

        /// <summary>
        /// Initializes a new instance of <see cref="TransactionSituationModel"/>.
        /// </summary>
        /// <param name="coreMainNodeDatabase">Origin database context.</param>
        /// <param name="coreReplicaNodeDatabase">Replica database context.</param>
        public TransactionSituationModel(CoreOrigin coreMainNodeDatabase, CoreReplica coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// Get a valid transaction situation
        /// </summary>
        /// <param name="transaction_situation"></param>
        /// <returns>TransactionTypeEntity</returns>
        public async Task<TransactionSituationEntity> GetTransactionSituationByIdAsync(int transaction_situation)
            => await _CoreReplicaNodeDatabase.TransactionSituation.FirstOrDefaultAsync(x => x.Id == transaction_situation && x.Active == true);

        /// <summary>
        /// Method to search for transaction situation
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(List<TransactionSituationView>, int)> GetTransactionSituationAsync(int page, int pageSize)
        {
            List<TransactionSituationView> transactionSituation = new();
            StringBuilder query = new();
            DynamicParameters parameters = new();

            query.Append("""
                select 
                    ts.id as Id,
                    ts.name as Name, 
                    ts.active as Active,
                    ts.description as Description,
                    count(*) over() as Records
                from payment.transaction_situation ts 
                where ts.active is true
                order by ts.id
                limit @limit
                offset @offset
            """);

            parameters.Add("limit", pageSize);
            parameters.Add("offset", (page - 1) * pageSize);

            var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();

            transactionSituation.AddRange(
                await connection.QueryAsync<TransactionSituationView>(query.ToString(), parameters)
            );

            int records = transactionSituation.Any() ? transactionSituation.FirstOrDefault().Records : 0;

            return (transactionSituation, records);
        }
    }
}