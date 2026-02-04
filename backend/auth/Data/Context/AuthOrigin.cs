using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace auth.Data.Context
{

    public class AuthOrigin : AuthDbContext
    {
        /// <summary>
        /// Class for connect on main PGSQL database for AUTH
        /// This context create connection only for ORIGIN, were we can use CRUD.
        /// <returns>Connectio to Auth database PGSQL in main node</returns>
        /// </summary>
        
        protected override string KeyNameConnection => "auth_pgsql_origin";

        public AuthOrigin(DbContextOptions<AuthOrigin> options, IConfiguration configuration) 
            : base(options, configuration)
        {
            
        }

    }
}