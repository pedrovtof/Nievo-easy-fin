using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Dapper;

namespace NievoEasyFin.Application.Models
{
    /// <summary>
    /// Class model for BankCardTypeModel
    /// </summary>
    public class BankCardTypeModel : BankCardTypeEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica? _CoreReplicaNodeDatabase;

        public BankCardTypeModel(CoreOrigin coreMainNodeDatabase, CoreReplica? coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// Search for a valid BankCardType by id or name
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">name</param>
        /// <returns>BankCardTypeEntity</returns>
        public async Task<BankCardTypeEntity> GetBankCardTypeByIdOrNameAsync(int id, string name)
            => await _CoreReplicaNodeDatabase.BankCardType.FirstOrDefaultAsync(x => x.Id == id && x.Active == true || x.Name == name && x.Active == true);

        /// <summary>
        /// Bring a list of valid bank card types
        /// </summary>
        /// <returns>List of BankCardTypeEntity</returns>
        public async Task<(List<BankCardTypeEntity>, int)> GetBankCardTypesAsync()
        {
            List<BankCardTypeEntity> items = new();

            StringBuilder sql = new();

            sql.Append(@"
                SELECT 
                    id, 
                    name, 
                    description, 
                    active,
                    created_at,
                    updated_at
                FROM accounts.bank_card_type
                WHERE active = true;
            ");

            var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();

            items.AddRange(
                await connection.QueryAsync<BankCardTypeEntity>(
                    sql.ToString()
                )
            );

            return (items, items.Count());
        }
    }
}
