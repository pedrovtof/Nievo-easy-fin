using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NievoEasyfin.Application.Data.Entities;

namespace NievoEasyfin.Application.Data.Context
{
    /// <summary>
    /// Class for create a contex with database
    /// This class is abstract and must be inherited by other classes
    /// This class can only use the database types defined in the configuration
    /// This class can only login on AUTH databases
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when the configuration is null</exception>
    /// <exception cref="ArgumentException">Thrown when the database type is invalid</exception>
    /// <returns>A new instance of the database context</returns>
    public abstract class AuthDbContext : DbContext
    {

        private IConfiguration _configuration;
        protected abstract string KeyNameConnection { get; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserStatusEntity> UserStatuses { get; set; }
        public DbSet<UserTypeEntity> UserTypes { get; set; }
        public DbSet<TokenConfigEntity> TokenConfig { get; set; }
        public DbSet<SsoProviderEntity> SsoProvider { get; set; }
        public DbSet<UserProviderSsoEntity> UserProvider { get; set; }

        protected AuthDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            var connectionString = _configuration.GetConnectionString(KeyNameConnection);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("[AuthDbContext][OnConfiguring] Invalid connection string variable KeyNameConnection is Null or Empty");
            }

            switch (KeyNameConnection)
            {
                case "auth_pgsql_replica":
                    optionsBuilder.UseNpgsql(connectionString);
                    break;
                case "auth_pgsql_origin":
                    optionsBuilder.UseNpgsql(connectionString);
                    break;
                default:
                    throw new ArgumentException("[AuthDbContext][OnConfiguring] Invalid database type, not configurated in AuthDbContext value => " + KeyNameConnection + " <=");
            }
            return;
        }
    }
}
