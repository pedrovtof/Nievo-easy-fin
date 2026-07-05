using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace NievoEasyFin.Application.Data.Context.Database;

/// <summary>
/// Class for connect on main PGSQL database for Core
/// This context create connection only for ORIGIN, were we can use CRUD.
/// <returns>Connectio to Core database PGSQL in main node</returns>
/// </summary>
public class CoreOrigin : EasyFinDbContext
{
    protected override string PGSQL_DATABASE_CONNECTION_STRING => DotNetEnv.Env.GetString("PGSQL_DATABASE_CORE_CONNECTION_STRING");

    public CoreOrigin(DbContextOptions<CoreOrigin> options, IConfiguration configuration)
        : base(options, configuration)
    {

    }
}
