using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NievoEasyfin.Application.Data.Context.Database
{
    /// <summary>
    /// Class for connect on main PGSQL database for AUTH
    /// This context create connection only for ORIGIN, were we can use CRUD.
    /// <returns>Connectio to Auth database PGSQL in main node</returns>
    /// </summary>
    public class AuthOrigin : AuthDbContext
    {
        protected override string KeyNameConnection => "auth_pgsql_origin";

        public AuthOrigin(DbContextOptions<AuthOrigin> options, IConfiguration configuration)
            : base(options, configuration)
        {

        }
    }
}
