using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Dapper;

namespace NievoEasyFin.Application.Models
{
    public class BankCardFlagModel : BankCardFlagEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica? _CoreReplicaNodeDatabase;

        public BankCardFlagModel(CoreOrigin coreMainNodeDatabase, CoreReplica? coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// Search for a valid BankCardFlag by name
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>BankCardFlagEntity</returns>
        public async Task<BankCardFlagEntity> GetBankCardFlagByName(string name)
            => await _CoreReplicaNodeDatabase.BankCardFlag.FirstOrDefaultAsync(x => x.Name == name && x.Active == true);

        /// <summary>
        /// Bring a list of valid bank card flags
        /// </summary>
        /// <returns>List of BankCardFlagEntity</returns>
        public async Task<(List<BankCardFlagEntity>, int)> GetBankCardFlagAsync(int page, int pageSize)
        {
            List<BankCardFlagEntity> items = new();

            StringBuilder sql = new();
            DynamicParameters param = new();

            sql.Append(@"
                select
                    bcf.name  as Name,
                    bcf.description as Description,
                    count(*) over() as Records
                from accounts.bank_card_flag bcf
                where bcf.active = true
            ");

            sql.Append(@"
                limit @limit
                offset @offset
            ");

            param.Add("limit", pageSize);
            param.Add("offset", (page - 1) * pageSize);

            var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();

            items.AddRange(
                await connection.QueryAsync<BankCardFlagEntity>(
                    sql.ToString(),
                    param
                )
            );

            return (items, items.Count());
        }
    }
}
