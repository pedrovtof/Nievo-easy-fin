using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace NievoEasyFin.Application.Data.Context.Database;

/// <summary>
/// Class for connect on RR node PGSQL database for Core
/// This context create connection only for RR node, were we can use READ_ONLY
/// </summary>
/// <returns>Connectio to Core database PGSQL in READ_REPLICA node</returns>
public class CoreReplica : EasyFinDbContext
{
    protected override string PGSQL_DATABASE_CONNECTION_STRING => DotNetEnv.Env.GetString("PGSQL_DATABASE_CORE_CONNECTION_READ_REPLICA_STRING");

    public CoreReplica(DbContextOptions<CoreReplica> options, IConfiguration configuration)
        : base(options, configuration)
    {

    }
}
