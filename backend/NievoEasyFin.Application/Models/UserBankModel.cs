using System.Reflection;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;

namespace NievoEasyFin.Application.Models
{
    public class UserBankModel : UserBankEntity
    {
        private readonly CoreOrigin _CoreMainNodeDatabase;

        private readonly CoreReplica _CoreReplicaNodeDatabase;

        public UserBankModel(CoreOrigin coreMainNodeDatabase, CoreReplica coreReplicaNodeDatabase)
        {
            _CoreMainNodeDatabase = coreMainNodeDatabase;
            _CoreReplicaNodeDatabase = coreReplicaNodeDatabase;
        }

        /// <summary>
        /// Search for a User Bank active
        /// </summary>
        /// <param name="userId">int</param>
        /// <param name="bankId">int</param>
        /// <returns>UserBankEntity</returns>
        public async Task<UserBankEntity> GetUserBankByUserAndBankAsync(int userId, int bankId)
            => await _CoreReplicaNodeDatabase.UserBank.FirstOrDefaultAsync(x => x.UserId == userId && x.BankId == bankId && x.Active == true);

        /// <summary>
        /// Search for all user banks active
        /// </summary>
        /// <param name="userId">int</param>
        /// <returns></returns>
        public async Task<List<UserBanksView>> GetUserBankByUserAsync(int userId)
        {
            StringBuilder query = new();
            DynamicParameters param = new();
            List<UserBanksView> userBanks = new();

            query.Append(@"
                select
                    b.name as Name,
                    b.bank_type as BankType,
                    ub.nick_name as NickName,
                    bt.name as BankTypeName
                from accounts.user_bank ub
                    inner join accounts.bank b
                        on ub.bank_id = b.id
                    inner join accounts.bank_type bt
                        on b.bank_type = bt.id
                where
                    ub.user_id = @userId
                    and ub.active = @Active
                    and b.active = @Active
                    and bt.active = @Active;
            ");

            param.Add("userId", userId);
            param.Add("Active", true);

            var connection = _CoreReplicaNodeDatabase.Database.GetDbConnection();

            userBanks.AddRange(
                await connection.QueryAsync<UserBanksView>(
                    query.ToString(),
                    param
                )
            );

            return userBanks;
        }

        /// <summary>
        /// Method to create a new user linked to a bank
        /// </summary>
        /// <param name="userId">int</param>
        /// <param name="nickname">string</param>
        /// <param name="bankId">int</param>
        /// <returns>UserBankEntity</returns>
        public async Task<UserBankEntity> CreateUserBankAsync(int userId, string nickname, int bankId)
        {
            UserBankEntity userBank = new()
            {
                UserId = userId,
                NickName = nickname,
                Active = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                BankId = bankId
            };

            await _CoreMainNodeDatabase.UserBank.AddAsync(userBank);
            await _CoreMainNodeDatabase.SaveChangesAsync();

            return userBank;
        }
    }
}
