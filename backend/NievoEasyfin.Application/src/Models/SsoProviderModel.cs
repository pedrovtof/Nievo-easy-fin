using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Data.Context.Database;
using Sprache;
using NievoEasyfin.Application.Data.Views;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NievoEasyfin.Application.Models
{
    public class SsoProviderModel : SsoProviderEntity
    {
        private static AuthOrigin _AuthMainNodeDatabase;

        private static AuthReplica? _AuthReplicaNodeDatabase;

        public SsoProviderModel(AuthOrigin authMainNodeDatabase, AuthReplica authReplicaNodeDatabase)
        {
            _AuthMainNodeDatabase = authMainNodeDatabase;
            _AuthReplicaNodeDatabase = authReplicaNodeDatabase;
        }

        /// <summary>
        /// Method to search in database the provider
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<SsoProviderEntity> GetProviderByNameAsync(string provider)
            => await _AuthReplicaNodeDatabase.SsoProvider.FirstOrDefaultAsync<SsoProviderEntity>(x => x.Name == provider && x.Active == true);

    }
}