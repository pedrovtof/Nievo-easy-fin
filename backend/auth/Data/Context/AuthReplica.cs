using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace auth.Data.Context
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