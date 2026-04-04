using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Data.Context.Database;
using Sprache;
using NievoEasyfin.Application.Data.Views;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;


namespace NievoEasyfin.Application.Models
{
    public class UserProviderSsoModel : UserProviderSsoEntity
    {
        private static AuthOrigin _AuthMainNodeDatabase;

        private static AuthReplica? _AuthReplicaNodeDatabase;

        public UserProviderSsoModel(AuthOrigin authMainNodeDatabase, AuthReplica authReplicaNodeDatabase)
        {
            _AuthMainNodeDatabase = authMainNodeDatabase;
            _AuthReplicaNodeDatabase = authReplicaNodeDatabase;
        }

        public async Task<UserProviderSsoEntity> CreateUserProviderSsoEntityAsync(int provider, int user, string sub)
        {
            UserProviderSsoEntity userProvider = new UserProviderSsoEntity()
            {
                Sub = sub,
                UserId = user,
                SsoProviderId = provider,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _AuthMainNodeDatabase.UserProvider.AddAsync(userProvider);
            await _AuthMainNodeDatabase.SaveChangesAsync();

            return userProvider;
        }

        /// <summary>
        /// Search userProviderSso by Sub and Provider
        /// </summary>
        /// <param name="sub">Unique id</param>
        /// <param name="provider">provider Id</param>
        /// <returns>UserProviderSsoEntity</returns>
        public async Task<UserProviderSsoEntity> GetUserProviderSsoBySubAndProviderAsync(string sub, int provider)
            => await _AuthReplicaNodeDatabase.UserProvider.FirstOrDefaultAsync<UserProviderSsoEntity>(x => x.Sub == sub && x.SsoProviderId == provider);

        /// <summary>
        ///  Search userProviderSso by Provider and User
        /// </summary>
        /// <param name="provider">provider Id</param>
        /// <param name="user">User Id</param>
        /// <returns>UserProviderSsoEntity</returns>
        public async Task<UserProviderSsoEntity> GetUserProviderSsoByProviderAndUserAsync(int provider, int user)
            => await _AuthReplicaNodeDatabase.UserProvider.FirstOrDefaultAsync<UserProviderSsoEntity>(x => x.SsoProviderId == provider && x.UserId == user);
    }
}