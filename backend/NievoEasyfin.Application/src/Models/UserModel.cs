using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Data.Context.Database;
using Sprache;
using NievoEasyfin.Application.Data.Views;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NievoEasyfin.Application.Models
{
    public class UserModel : UserEntity
    {

        private static AuthOrigin _AuthMainNodeDatabase;

        private static AuthReplica? _AuthReplicaNodeDatabase;

        public UserModel(AuthOrigin authMainNodeDatabase, AuthReplica authReplicaNodeDatabase)
        {
            _AuthMainNodeDatabase = authMainNodeDatabase;
            _AuthReplicaNodeDatabase = authReplicaNodeDatabase;
        }

        /// <summary>
        /// Method to create user
        /// </summary>
        /// <param name="name">Request name</param>
        /// <param name="password">Hash password from request</param>
        /// <param name="email">Request Email</param>
        /// <param name="statusId">User status Id</param>
        /// <param name="phone">User phone</param>
        /// <returns>UserView</returns>
        public async Task<UserView> CreateUserAsync(string name, string password, string email, int statusId = 1, int? phone = null)
        {

            UserEntity user = new UserEntity()
            {
                Name = name,
                Email = email,
                Phone = phone,
                Password = password,
                StatusId = statusId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _AuthMainNodeDatabase.Users.AddAsync(user);
            await _AuthMainNodeDatabase.SaveChangesAsync();

            return new UserView()
            {
                Name = user.Name,
                Email = user.Email,
                StatusId = $"{user.StatusId}",
                Phone = user.Phone,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
            => await _AuthMainNodeDatabase.Users.FirstOrDefaultAsync<UserEntity>(x => x.Email == email);
    }
}