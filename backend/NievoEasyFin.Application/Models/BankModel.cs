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
        /// <param name="name"></param>
        /// <param name="bankType"></param>
        /// <returns></returns>
        public async Task<BankEntity> GetBankByNameAndTypeAsync(string name, int bankType)
            => await _CoreReplicaNodeDatabase.Bank.FirstOrDefaultAsync(x => x.Name == name && x.BankType == bankType);
    }
}