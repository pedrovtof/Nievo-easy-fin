using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Models;
using NievoEasyFin.Application.Services.Base;
using NievoEasyFin.Tests.Mocks.Helpers;
using StackExchange.Redis;
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

public class CoreOriginTest : CoreOrigin
{
    public CoreOriginTest(DbContextOptions<CoreOrigin> options, IConfiguration configuration) : base(options, configuration) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnType(null);
            }
        }
        modelBuilder.Entity<UsersAcceptedTermsEntity>().Ignore(e => e.RequestDetails);
    }
}

public class CoreReplicaTest : CoreReplica
{
    public CoreReplicaTest(DbContextOptions<CoreReplica> options, IConfiguration configuration) : base(options, configuration) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnType(null);
            }
        }
        modelBuilder.Entity<UsersAcceptedTermsEntity>().Ignore(e => e.RequestDetails);
    }
}

public abstract class AccountsServiceTestBase : IDisposable
{
    private SqliteConnection _connection;
    protected readonly ITestOutputHelper Output;

    protected AccountsServiceTestBase(ITestOutputHelper output)
    {
        Output = output;
        DotNetEnv.Env.Load(".env");
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }

    protected (CoreOrigin, CoreReplica) CreateSharedCoreContexts()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        using (var cmd = _connection.CreateCommand())
        {
            cmd.CommandText = "ATTACH DATABASE ':memory:' AS user_details; ATTACH DATABASE ':memory:' AS journey; ATTACH DATABASE ':memory:' AS accounts;";
            cmd.ExecuteNonQuery();
        }

        var optionsOrigin = new DbContextOptionsBuilder<CoreOrigin>().UseSqlite(_connection).Options;
        var optionsReplica = new DbContextOptionsBuilder<CoreReplica>().UseSqlite(_connection).Options;

        var configuration = Substitute.For<IConfiguration>();

        var origin = new CoreOriginTest(optionsOrigin, configuration);
        var replica = new CoreReplicaTest(optionsReplica, configuration);

        origin.Database.EnsureCreated();

        using (var cmd = _connection.CreateCommand())
        {
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS accounts.bank (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    bank_type INTEGER,
                    active INTEGER,
                    created_at TEXT,
                    updated_at TEXT
                );
                CREATE TABLE IF NOT EXISTS accounts.bank_type (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    description TEXT,
                    active INTEGER,
                    created_at TEXT,
                    updated_at TEXT
                );
                CREATE TABLE IF NOT EXISTS accounts.user_bank (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    nick_name TEXT,
                    user_id INTEGER,
                    bank_id INTEGER,
                    active INTEGER,
                    created_at TEXT,
                    updated_at TEXT
                );
                CREATE TABLE IF NOT EXISTS accounts.bank_card (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    bank_id INTEGER,
                    name TEXT,
                    card_type INTEGER,
                    active INTEGER,
                    created_at TEXT,
                    updated_at TEXT
                );
                CREATE TABLE IF NOT EXISTS accounts.bank_card_type (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    description TEXT,
                    active INTEGER,
                    created_at TEXT,
                    updated_at TEXT
                );
            ";
            cmd.ExecuteNonQuery();
        }

        return (origin, replica);
    }

    protected AccountsService CreateService(CoreOrigin origin, CoreReplica replica, AuthOrigin authOrigin, AuthReplica authReplica)
    {
        var bankModel = new BankModel(origin, replica);
        var bankTypeModel = new BankTypeModel(origin, replica);
        var userModel = new UserModel(authOrigin, authReplica);
        var userBankModel = new UserBankModel(origin, replica);
        var bankCardModel = new BankCardModel(origin, replica);
        var bankCardTypeModel = new BankCardTypeModel(origin, replica);
        var userBankCardModel = new UserBankCardModel(origin, replica);

        var dbMock = new Mock<IDatabase>();
        dbMock.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(RedisValue.Null);
        dbMock.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(true);

        var cacheService = MockHelper.CreateMockedCacheService(dbMock);

        return new AccountsService(bankModel, cacheService, bankTypeModel, userModel, userBankModel, bankCardModel, bankCardTypeModel, userBankCardModel);
    }

    protected async System.Threading.Tasks.Task SyncCoreToAttachedDatabasesAsync(CoreOrigin context)
    {
        var connection = context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = @"
                INSERT OR REPLACE INTO accounts.bank SELECT * FROM main.bank;
                INSERT OR REPLACE INTO accounts.bank_type SELECT * FROM main.bank_type;
                INSERT OR REPLACE INTO accounts.user_bank SELECT * FROM main.user_bank;
                INSERT OR REPLACE INTO accounts.bank_card SELECT * FROM main.bank_card;
                INSERT OR REPLACE INTO accounts.bank_card_type SELECT * FROM main.bank_card_type;
            ";
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
