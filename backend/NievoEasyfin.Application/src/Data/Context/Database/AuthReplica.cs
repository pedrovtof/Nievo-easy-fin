using Microsoft.EntityFrameworkCore;
using NievoEasyfin.Application.Data.Context;
using Microsoft.Extensions.Configuration;

namespace NievoEasyfin.Application.Data.Context.Database
{
    public class AuthReplica : AuthDbContext
    {
        /// <summary>
        /// Class for connect on RR node PGSQL database for AUTH
        /// This context create connection only for RR node, were we can use READ_ONLY
        /// <returns>Connectio to Auth database PGSQL in READ_REPLICA node</returns>
        /// <summary>

        protected override string KeyNameConnection => "auth_pgsql_replica";

        public AuthReplica(DbContextOptions<AuthReplica> options, IConfiguration configuration)
            : base(options, configuration)
        {

        }
    }
}