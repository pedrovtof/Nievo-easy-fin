using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Data.Context.Database;
using Microsoft.EntityFrameworkCore;

namespace NievoEasyfin.Application.Models
{
    /// <summary>
    /// Class model for user
    /// </summary>
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
        public async Task<UserEntity> CreateUserAsync(string name, string password, string email, int statusId = 1, int? phone = null)
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

            return user;
        }

        /// <summary>
        /// Method to create user-SSO
        /// </summary>
        /// <param name="name">user from name</param>
        /// <param name="email">user from email</param>
        /// <param name="sub">user from sub</param>
        /// <param name="statusId">User status Id</param>
        /// <param name="phone">User phone</param>
        /// <returns></returns>
        public async Task<UserEntity> CreateUserSsoAsync(string name, string email, string sub, int statusId = 1, int? phone = null)
        {
            UserEntity user = new UserEntity()
            {
                Name = name,
                Email = email,
                Phone = phone,
                Password = null,
                StatusId = statusId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _AuthMainNodeDatabase.Users.AddAsync(user);
            await _AuthMainNodeDatabase.SaveChangesAsync();

            return user;
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
            => await _AuthReplicaNodeDatabase.Users.FirstOrDefaultAsync<UserEntity>(x => x.Email == email);
    }
}
