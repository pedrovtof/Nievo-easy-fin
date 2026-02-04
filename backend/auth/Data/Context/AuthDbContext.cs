using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using auth.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace auth.Data.Context
{

    public abstract class AuthDbContext : DbContext
    {
        /// <summary>
        /// Class for create a contex with database
        /// This class is abstract and must be inherited by other classes
        /// This class can only use the database types defined in the configuration
        /// This class can only login on AUTH databases
        /// <param name="configuration">Configuration for the database context</param>
        /// <param name="KeyNameConnection">Key for chose the connection</param>
        /// <exception cref="ArgumentNullException">Thrown when the configuration is null</exception>
        /// <exception cref="ArgumentException">Thrown when the database type is invalid</exception>
        /// <returns>A new instance of the database context</returns>
        /// </summary>


        private IConfiguration _configuration;
        protected abstract string KeyNameConnection {get;}
        public DbSet<UserData> Users { get; set; }
        public DbSet<UserStatusData> UserStatuses { get; set; }
        public DbSet<UserTypeData> UserTypes { get; set; }
        public DbSet<UserPasswordHistoryData> UserPasswordHistories { get; set; }
        public DbSet<TokenConfig> TokenConfig { get; set; }
 
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

            //var typeDatabase = _configuration["TypeDatabase"] ?? "";
            var connectionString = _configuration.GetConnectionString(KeyNameConnection);

            if(string.IsNullOrEmpty(connectionString))
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