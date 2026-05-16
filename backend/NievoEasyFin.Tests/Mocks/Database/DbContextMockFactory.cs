using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NievoEasyFin.Application.Data.Context.Database;
using NSubstitute;
using Microsoft.Data.Sqlite;
using NievoEasyFin.Application.Data.Context;

namespace NievoEasyFin.Tests.Mocks.Database;

public static class DbContextMockFactory
{
    private static SqliteConnection? _sharedConnection;

    private static SqliteConnection GetSharedConnection()
    {
        if (_sharedConnection == null)
        {
            _sharedConnection = new SqliteConnection("DataSource=:memory:");
            _sharedConnection.Open();
        }
        return _sharedConnection;
    }

    public static AuthOrigin CreateAuthOrigin(string? dbName = null)
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        // Attach databases to simulate schemas for Dapper
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = "ATTACH DATABASE ':memory:' AS user_details; ATTACH DATABASE ':memory:' AS journey;";
            cmd.ExecuteNonQuery();
        }

        var options = new DbContextOptionsBuilder<AuthOrigin>()
            .UseSqlite(connection)
            .Options;

        var configuration = Substitute.For<IConfiguration>();
        configuration.GetConnectionString("auth_pgsql_origin").Returns("Host=localhost;Database=test;Username=test;Password=test");

        var context = new AuthOriginTest(options, configuration);
        context.Database.EnsureCreated();

        return context;
    }

    public static AuthReplica CreateAuthReplica(string? dbName = null)
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = "ATTACH DATABASE ':memory:' AS user_details; ATTACH DATABASE ':memory:' AS journey;";
            cmd.ExecuteNonQuery();
        }

        var options = new DbContextOptionsBuilder<AuthReplica>()
            .UseSqlite(connection)
            .Options;

        var configuration = Substitute.For<IConfiguration>();
        configuration.GetConnectionString("auth_pgsql_replica").Returns("Host=localhost;Database=test;Username=test;Password=test");

        var context = new AuthReplicaTest(options, configuration);
        context.Database.EnsureCreated();

        return context;
    }

    // Share connection for origin/replica in tests
    public static (AuthOrigin, AuthReplica) CreateSharedAuthContexts()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = "ATTACH DATABASE ':memory:' AS user_details; ATTACH DATABASE ':memory:' AS journey;";
            cmd.ExecuteNonQuery();
        }

        var optionsOrigin = new DbContextOptionsBuilder<AuthOrigin>().UseSqlite(connection).Options;
        var optionsReplica = new DbContextOptionsBuilder<AuthReplica>().UseSqlite(connection).Options;

        var configuration = Substitute.For<IConfiguration>();

        var origin = new AuthOriginTest(optionsOrigin, configuration);
        var replica = new AuthReplicaTest(optionsReplica, configuration);

        origin.Database.EnsureCreated();

        // Manually create tables in attached databases for Dapper and EF Core consistency in SQLite
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = @"
                CREATE TABLE user_details.""user"" (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    email TEXT,
                    phone INTEGER,
                    status_id INTEGER,
                    password TEXT,
                    created_at TEXT,
                    updated_at TEXT
                );
                CREATE TABLE user_details.user_status (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    description TEXT,
                    active INTEGER,
                    created_at TEXT,
                    updated_at TEXT
                );
                CREATE TABLE user_details.user_type (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    description TEXT,
                    created_at TEXT,
                    updated_at TEXT
                );
                CREATE TABLE journey.sso_provider (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    active INTEGER,
                    name TEXT,
                    description TEXT,
                    created_at TEXT,
                    updated_at TEXT
                );
                CREATE TABLE journey.user_provider_sso (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    user_id INTEGER,
                    sso_provider_id INTEGER,
                    sub TEXT,
                    created_at TEXT,
                    updated_at TEXT
                );
            ";
            cmd.ExecuteNonQuery();
        }

        return (origin, replica);
    }

    public static async Task SyncToAttachedDatabasesAsync(AuthDbContext context)
    {
        var connection = context.Database.GetDbConnection();
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = @"
                INSERT OR REPLACE INTO user_details.""user"" SELECT * FROM main.user;
                INSERT OR REPLACE INTO user_details.user_status SELECT * FROM main.user_status;
                INSERT OR REPLACE INTO user_details.user_type SELECT * FROM main.user_type;
                INSERT OR REPLACE INTO journey.sso_provider SELECT * FROM main.sso_provider;
                INSERT OR REPLACE INTO journey.user_provider_sso SELECT * FROM main.user_provider_sso;
            ";
            await cmd.ExecuteNonQueryAsync();
        }
    }
}

public class AuthOriginTest : AuthOrigin
{
    public AuthOriginTest(DbContextOptions<AuthOrigin> options, IConfiguration configuration) : base(options, configuration) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // We keep schemas so they match the ATTACHed databases
        FixForSqlite(modelBuilder);
    }

    private void FixForSqlite(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Do NOT SetSchema(null) here, let it be user_details or journey
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnType(null);
            }
        }
    }
}

public class AuthReplicaTest : AuthReplica
{
    public AuthReplicaTest(DbContextOptions<AuthReplica> options, IConfiguration configuration) : base(options, configuration) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        FixForSqlite(modelBuilder);
    }

    private void FixForSqlite(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnType(null);
            }
        }
    }
}
