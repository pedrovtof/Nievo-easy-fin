using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Models
{
    /// <summary>
    /// Bank Model
    /// </summary>
    public class BankModel : BankEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica _CoreReplicaNodeDatabase;

        public BankModel(CoreOrigin coreMainNodeDatabase, CoreReplica coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// List banks
        /// </summary>
        /// <returns>List BanksViews</returns>
        public async Task<(List<BanksViews>, int)> GetBanksAsync(int page, int pageSize)
        {
            List<BanksViews> banks = new();
            StringBuilder query = new();
            DynamicParameters parameters = new();

            query.Append("""
                select
                    b."name" as Name,
                    b.active as BankActive,
                    b.bank_type as BankType,
                    bt."name" as BankTypeName,
                    bt.description as Description,
                    bt.active as BankTypeActive,
                    count(*) over() as Records
                from
                    accounts.bank b
                        left join accounts.bank_type bt 
                            on b.bank_type = bt.id 
                where 
                    b.active = true
                    and bt.active = true
                    limit @limit
                    offset @offset
            """);

            parameters.Add("limit", pageSize);
            parameters.Add("offset", (page - 1) * pageSize);

            var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();

            banks.AddRange(
                await connection.QueryAsync<BanksViews>(query.ToString(), parameters)
            );

            int records = banks[0]?.Records ?? 0;

            return (banks, records);
        }

        /// <summary>
        /// Return database entity
        /// </summary>
        /// <param name="name">Bank name</param>
        /// <param name="bankType">Bank type</param>
        /// <returns>BankEntity</returns>
        public async Task<BankEntity> GetBankByNameAndTypeAsync(string name, int bankType)
            => await _CoreReplicaNodeDatabase.Bank.FirstOrDefaultAsync(x => x.Name == name && x.BankType == bankType && x.Active == true);

        /// <summary>
        ///  Create the entity Bank in the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bankType"></param>
        /// <param name="active"></param>
        /// <returns>BankEntity</returns>
        public async Task<BankEntity> CreateBankAsync(string name, int bankType, bool active = true)
        {
            BankEntity bank = new()
            {
                Name = name,
                BankType = bankType,
                Active = active,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _CoreMainNodeDatabase.Bank.AddAsync(bank);
            await _CoreMainNodeDatabase.SaveChangesAsync();

            return bank;
        }
    }
}
