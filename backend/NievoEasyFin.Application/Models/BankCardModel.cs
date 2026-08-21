using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Text;
using NievoEasyFin.Application.Data.Views;

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
        /// Search for bankCard using BankID and CardID
        /// </summary>
        /// <param name="bankId">int</param>
        /// <param name="CardId">int</param>
        /// <returns>BankCardEntity</returns>
        public async Task<BankCardEntity> GetBankCardByBankIdAndCardId(int bankId, int CardId)
            => await _CoreReplicaNodeDatabase.BankCard.FirstOrDefaultAsync(x => x.BankId == bankId && x.Id == CardId && x.Active == true);

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

        /// <summary>
        /// Method to search for bank cards
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="bankId"></param>
        /// <param name="cardTypeId"></param>
        /// <returns></returns>
        public async Task<(List<BankCardView>, int)> GetBankCardsAsync(int page, int pageSize, int? bankId, int? cardTypeId)
        {
            List<BankCardView> items = new();

            StringBuilder query = new();
            DynamicParameters param = new();

            query.Append(@"
                select
                    b.name as Bank,
                    bct.name as CardType,
                    bc.name as CardName,
                    bc.id as Id,
                    count(*) over() as Records
                from accounts.bank_card bc
                    inner join accounts.bank b 
                        on bc.bank_id = b.id
                    inner join accounts.bank_card_type bct 
                        on bc.card_type = bct.id 
                where b.active = true
                    and bc.active = true
                    and bct.active = true
            ");

            if (bankId != null)
            {
                query.Append(@"
                    and b.id = @bankId
                ");
                param.Add("bankId", bankId);
            }

            if (cardTypeId != null)
            {
                query.Append(@"
                    and bct.id = @cardTypeId
                ");
                param.Add("cardTypeId", cardTypeId);
            }

            query.Append(@"
                limit @limit
                offset @offset
            ");

            param.Add("limit", pageSize);
            param.Add("offset", (page - 1) * pageSize);

            var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();
            items.AddRange(
                await connection.QueryAsync<BankCardView>(
                    query.ToString(),
                    param
                )
            );

            return (items, items.Any() ? items.FirstOrDefault().Records : 0);
        }
    }
}
