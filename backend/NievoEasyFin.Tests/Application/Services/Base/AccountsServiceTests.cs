using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Models;
using NievoEasyFin.Application.Services.Base;
using NievoEasyFin.Tests.Mocks.Helpers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
namespace NievoEasyFin.Tests.Application.Services.Base;

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

public class AccountsServiceTests : IDisposable
{
    private SqliteConnection _connection;

    public AccountsServiceTests()
    {
        DotNetEnv.Env.Load(".env");
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }

    private (CoreOrigin, CoreReplica) CreateSharedCoreContexts()
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
            ";
            cmd.ExecuteNonQuery();
        }

        return (origin, replica);
    }

    private AccountsService CreateService(CoreOrigin origin, CoreReplica replica)
    {
        var bankModel = new BankModel(origin, replica);
        var bankTypeModel = new BankTypeModel(origin, replica);

        var dbMock = new Mock<IDatabase>();
        dbMock.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(RedisValue.Null);
        dbMock.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(true);

        var cacheService = MockHelper.CreateMockedCacheService(dbMock);

        return new AccountsService(bankModel, cacheService, bankTypeModel);
    }

    [Fact]
    public async Task PostAccountsBanks_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostAccountsBanksRequest { Name = "", BankType = 0 };
        var (origin, replica) = CreateSharedCoreContexts();
        var service = CreateService(origin, replica);

        // Act
        var result = await service.PostAccountsBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Name' must not be empty."));
        response.Messages.Should().Contain(e => e.Contains("'Bank Type' must not be empty.") || e.Contains("'Bank Type' must be greater than or equal to '1'."));
    }

    [Fact]
    public async Task PostAccountsBanks_WhenBankAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostAccountsBanksRequest { Name = "Nubank", BankType = 1 };
        var (origin, replica) = CreateSharedCoreContexts();

        // Seed bank
        var existingBank = new BankEntity { Name = "Nubank", BankType = 1, Active = true, CreatedAt = DateTime.UtcNow };
        origin.Bank.Add(existingBank);
        await origin.SaveChangesAsync();

        var service = CreateService(origin, replica);

        // Act
        var result = await service.PostAccountsBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_400_BANK_ALREADY_EXISTS.GetDescription()));
    }

    [Fact]
    public async Task PostAccountsBanks_WhenBankTypeInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostAccountsBanksRequest { Name = "Nubank", BankType = 99 };
        var (origin, replica) = CreateSharedCoreContexts();
        var service = CreateService(origin, replica);

        // Act
        var result = await service.PostAccountsBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_400_BANKTYPE_INVALID.GetDescription()));
    }

    [Fact]
    public async Task PostAccountsBanks_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = new PostAccountsBanksRequest { Name = "Nubank", BankType = 1 };
        var (origin, replica) = CreateSharedCoreContexts();

        // Seed bank type
        var existingBankType = new BankTypeEntity { Id = 1, Name = "Conta Corrente", Description = "Conta Corrente", Active = true, CreatedAt = DateTime.UtcNow };
        origin.BankType.Add(existingBankType);
        await origin.SaveChangesAsync();

        var service = CreateService(origin, replica);

        // Act
        var result = await service.PostAccountsBanks(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();
        
        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().Be(EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_200_CREATED.GetDescription());

        // Verify bank was created in database
        var bankInDb = await replica.Bank.FirstOrDefaultAsync(b => b.Name == "Nubank" && b.BankType == 1);
        bankInDb.Should().NotBeNull();
        bankInDb!.Active.Should().BeTrue();
    }
}
