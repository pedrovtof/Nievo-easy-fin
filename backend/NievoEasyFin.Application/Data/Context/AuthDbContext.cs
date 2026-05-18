using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Application.Data.Context;

/// <summary>
/// Class for create a contex with database
/// This class is abstract and must be inherited by other classes
/// Can only use the database types defined in the configuration
/// Can only login on AUTH databases
/// </summary>
/// <exception cref="ArgumentNullException">Thrown when the configuration is null</exception>
/// <exception cref="ArgumentException">Thrown when the database type is invalid</exception>
/// <returns>A new instance of the database context</returns>
public abstract class AuthDbContext : DbContext
{
    private IConfiguration _configuration;
    protected abstract string PGSQL_DATABASE_AUTH_CONNECTION_STRING { get; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserStatusEntity> UserStatuses { get; set; }
    public DbSet<UserTypeEntity> UserTypes { get; set; }
    public DbSet<SsoProviderEntity> SsoProvider { get; set; }
    public DbSet<UserProviderSsoEntity> UserProvider { get; set; }

    protected AuthDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Method to configure connection for database
    /// </summary>
    /// <param name="optionsBuilder"></param>
    /// <exception cref="ArgumentException"></exception>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;

        string connectionString = PGSQL_DATABASE_AUTH_CONNECTION_STRING;

        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentException("[AuthDbContext][OnConfiguring] Invalid connection string variable PGSQL_DATABASE_AUTH_CONNECTION_STRING is Null or Empty");

        try
        {
            optionsBuilder.UseNpgsql(connectionString);
            return;
        }
        catch
        {
            throw new ArgumentException($"[AuthDbContext][OnConfiguring] connection string");
        }
    }
}
