using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Text;

namespace NievoEasyFin.Application.Models
{
    /// <summary>
    /// Class model for BankCardModel
    /// </summary>
    public class BankCardModel : BankCardEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica? _CoreReplicaNodeDatabase;

        public BankCardModel(CoreOrigin coreMainNodeDatabase, CoreReplica? coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// Search for BankCard using ID,TYPE and NAME
        /// </summary>
        /// <param name="bankId">bankId</param>
        /// <param name="cardTypeId">cardTypeId</param>
        /// <param name="name">Name</param>
        /// <returns>BankCardEntity</returns>
        public async Task<BankCardEntity> GetBankCardByBankIdAndCardTypeAndNameAsync(int bankId, int cardTypeId, string name)
            => await _CoreReplicaNodeDatabase.BankCard.FirstOrDefaultAsync(x => x.BankId == bankId && x.CardType == cardTypeId && x.Name == name);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="cardTypeId"></param>
        /// <param name="name"></param>
        /// <returns>BankCardEntity</returns>
        public async Task<BankCardEntity> CreateBankCardAsync(int bankId, int cardTypeId, string name)
        {
            BankCardEntity bankCardEntity = new()
            {
                BankId = bankId,
                CardType = cardTypeId,
                Name = name,
                Active = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _CoreMainNodeDatabase.BankCard.AddAsync(bankCardEntity);
            await _CoreMainNodeDatabase.SaveChangesAsync();

            return bankCardEntity;
        }
    }
}
