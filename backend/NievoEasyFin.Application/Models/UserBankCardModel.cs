
using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
    }
}
