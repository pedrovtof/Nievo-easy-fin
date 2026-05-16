using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace NievoEasyFin.Application.Data.Context.Database;

/// <summary>
/// Class for connect on RR node PGSQL database for AUTH
/// This context create connection only for RR node, were we can use READ_ONLY
/// </summary>
/// <returns>Connectio to Auth database PGSQL in READ_REPLICA node</returns>
public class AuthReplica : AuthDbContext
{
    protected override string PGSQL_DATABASE_AUTH_CONNECTION_STRING => DotNetEnv.Env.GetString("PGSQL_DATABASE_AUTH_CONNECTION_READ_REPLICA_STRING");

    public AuthReplica(DbContextOptions<AuthReplica> options, IConfiguration configuration)
        : base(options, configuration)
    {

    }
}
