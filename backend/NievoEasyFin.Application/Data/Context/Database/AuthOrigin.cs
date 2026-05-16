using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace NievoEasyFin.Application.Data.Context.Database;

/// <summary>
/// Class for connect on main PGSQL database for AUTH
/// This context create connection only for ORIGIN, were we can use CRUD.
/// <returns>Connectio to Auth database PGSQL in main node</returns>
/// </summary>
public class AuthOrigin : AuthDbContext
{
    protected override string PGSQL_DATABASE_AUTH_CONNECTION_STRING => DotNetEnv.Env.GetString("PGSQL_DATABASE_AUTH_CONNECTION_STRING");

    public AuthOrigin(DbContextOptions<AuthOrigin> options, IConfiguration configuration)
        : base(options, configuration)
    {

    }
}
