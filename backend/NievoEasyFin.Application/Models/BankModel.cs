using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Models
{
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