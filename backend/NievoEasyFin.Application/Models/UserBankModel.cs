using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;

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
