
using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Models
{
    public class UserBankCardModel : UserBankCardEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica _CoreReplicaNodeDatabase;

        public UserBankCardModel(CoreOrigin coreMainNodeDatabase, CoreReplica coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// Create a user bank card for the current user
        /// </summary>
        /// <param name="bankId">int</param>
        /// <param name="cardId">int</param>
        /// <param name="userId">int</param>
        /// <param name="cardUserName">string</param>
        /// <param name="expireAt">datetime</param>
        /// <returns>UserBankCardEntity</returns>
        public async Task<UserBankCardEntity> CreateUserBankCard(int bankId, int cardId, int userId, string cardUserName, DateTime expireAt)
        {
            UserBankCardEntity userBankCard = new()
            {
                Active = true,
                CardId = cardId,
                BankId = bankId,
                UserId = userId,
                ExpiredAt = DateTime.SpecifyKind(expireAt, DateTimeKind.Unspecified),
                Name = cardUserName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _CoreMainNodeDatabase.UserBankCard.AddAsync(userBankCard);
            await _CoreMainNodeDatabase.SaveChangesAsync();

            return userBankCard;
        }

        /// <summary>
        /// Search for a list of user bank card
        /// </summary>
        /// <param name="page">int</param>
        /// <param name="pageSize">int</param>
        /// <param name="bankId">int</param>
        /// <param name="userId">int</param>
        /// <param name="active">bool</param>
        /// <param name="flag">string</param>
        /// <returns>UserBankCardView</returns>
        public async Task<(List<UserBankCardView>, int)> GetUserBankCard(int page, int pageSize, int? bankId, int userId, bool active, string? flag)
        {
            List<UserBankCardView> items = new();
            StringBuilder query = new();
            DynamicParameters param = new();

            query.Append(@"
                select 
                    ubc.id as UserBankCardId,
                    ubc.name  as UserBankCardName,
                    ubc.active  as Active,
                    ubc.expired_at as ExpiredAt,
                    b.name as BankName,
                    bc.name as BankCardName,
                    bct.name  as BankCardType,
                    bcf.name as BankCardFlag,
                    count(*) over() as Records
                from accounts.user_bank_card ubc
                    inner join accounts.bank b 
                        on ubc.bank_id = b.id 
                    inner join accounts.bank_card bc 
                        on ubc.card_id = bc.id 
                            and ubc.bank_id  = bc.bank_id
                    inner join accounts.bank_card_type bct 
                        on bc.card_type = bct.id 
                    inner join accounts.bank_card_flag bcf 
    	                on bc.flag_id = bcf.id 
                where  ubc.user_id = @userId
                    and ubc.active = @active
                    and b.active = true
                    and bc.active = true
                    and bct.active = true
                    and bcf.active = true
            ");

            param.Add("userId", userId);

            param.Add("active", active);

            if (bankId != null)
            {
                query.Append("and b.id = @bankId ");
                param.Add("bankId", bankId);
            }

            if (flag != null)
            {
                query.Append("and bcf.name = @flag ");
                param.Add("flag", flag);
            }

            query.Append(@"
                limit @limit
                offset @offset
            ");

            param.Add("limit", pageSize);
            param.Add("offset", (page - 1) * pageSize);

            var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();
            items.AddRange(
                await connection.QueryAsync<UserBankCardView>(
                    query.ToString(),
                    param
                )
            );

            return (items, items.Any() ? items.FirstOrDefault().Records : 0);
        }
    }
}
