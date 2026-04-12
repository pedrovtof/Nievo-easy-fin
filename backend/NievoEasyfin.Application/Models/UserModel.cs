using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Data.Context.Database;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Dapper;

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

        /// <summary>
        /// Method to get user by email
        /// </summary>
        /// <param name="email">string email</param>
        /// <returns>UserEntity</returns>
        public async Task<UserEntity> GetUserByEmailAsync(string email)
            => await _AuthReplicaNodeDatabase.Users.FirstOrDefaultAsync<UserEntity>(x => x.Email == email);

        /// <summary>
        /// Method to get user by providerId and subId
        /// </summary>
        /// <param name="subId">string sub</param>
        /// <param name="providerId">int provider.id</param>
        /// <returns>UserEntity</returns>
        public async Task<UserEntity> GetUserByProviderSubAndIdAsync(string subId, int providerId)
        {
            var query = new StringBuilder();
            var parameters = new DynamicParameters();

            query.Append("""
                SELECT 
                    u.id, "name", email, phone, status_id, u.created_at, u.updated_at, password
                FROM
                    user_details."user" u
                INNER JOIN journey.user_provider_sso ups 
                    on u.id  = ups.user_id 
                WHERE 1=1
                    and ups.sso_provider_id  = @providerId
                    and ups.sub = @subId
            """);

            parameters.Add("providerId", providerId);
            parameters.Add("subId", $"{subId}");

            var connection = _AuthReplicaNodeDatabase.Database.GetDbConnection();

            var dataFrame = await connection.QueryFirstAsync<UserEntity>(
                query.ToString(),
                parameters
            );

            return dataFrame;
        }
    }
}
