using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Models
{
    public class BankTypeModel : BankTypeEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica _CoreReplicaNodeDatabase;

        public BankTypeModel(CoreOrigin coreMainNodeDatabase, CoreReplica coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// Return database entity bankType
        /// </summary>
        public async Task<BankTypeEntity> GetBankTypeByNameAsync(int id)
        {
            var entity = await _CoreReplicaNodeDatabase.BankType.FirstOrDefaultAsync(x => x.Id == id && x.Active == true);
            return entity;
        }
    }
}