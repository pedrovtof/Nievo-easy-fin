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
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
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
                CREATE TABLE IF NOT EXISTS accounts.user_bank (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    nick_name TEXT,
                    user_id INTEGER,
                    bank_id INTEGER,
                    active INTEGER,
                    created_at TEXT,
                    updated_at TEXT
                );
            ";
            cmd.ExecuteNonQuery();
        }

        return (origin, replica);
    }

    private AccountsService CreateService(CoreOrigin origin, CoreReplica replica, AuthOrigin authOrigin, AuthReplica authReplica)
    {
        var bankModel = new BankModel(origin, replica);
        var bankTypeModel = new BankTypeModel(origin, replica);
        var userModel = new UserModel(authOrigin, authReplica);
        var userBankModel = new UserBankModel(origin, replica);

        var dbMock = new Mock<IDatabase>();
        dbMock.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(RedisValue.Null);
        dbMock.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(true);

        var cacheService = MockHelper.CreateMockedCacheService(dbMock);

        return new AccountsService(bankModel, cacheService, bankTypeModel, userModel, userBankModel);
    }

    [Fact]
    public async Task PostAccountsBanks_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = PostAccountsBanksRequestFaker.Create().Generate();
        request.Name = "";
        request.BankType = 0;
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

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
        var request = PostAccountsBanksRequestFaker.Create().Generate();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed bank
        var existingBank = BankEntityFaker.Create().Generate();
        existingBank.Name = request.Name;
        existingBank.BankType = request.BankType;
        origin.Bank.Add(existingBank);
        await origin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

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
        var request = PostAccountsBanksRequestFaker.Create().Generate();
        request.BankType = 99;
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

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
        var request = PostAccountsBanksRequestFaker.Create().Generate();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed bank type
        var existingBankType = new BankTypeEntity { Id = request.BankType, Name = "Conta Corrente", Description = "Conta Corrente", Active = true, CreatedAt = DateTime.UtcNow };
        origin.BankType.Add(existingBankType);
        await origin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostAccountsBanks(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().Be(EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_200_CREATED.GetDescription());

        // Verify bank was created in database
        var bankInDb = await replica.Bank.FirstOrDefaultAsync(b => b.Name == request.Name && b.BankType == request.BankType);
        bankInDb.Should().NotBeNull();
        bankInDb!.Active.Should().BeTrue();
    }

    [Fact]
    public async Task PostUserBanks_WhenBankTypeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = PostUserBanksRequestFaker.Create().Generate();
        request.BankType = 0;
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Bank Type' must be greater than or equal to '1'."));
    }

    [Fact]
    public async Task PostUserBanks_WhenBankNameIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = PostUserBanksRequestFaker.Create().Generate();
        request.BankName = "";
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Bank Name' must not be empty."));
    }

    [Fact]
    public async Task PostUserBanks_WhenEmailIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = PostUserBanksRequestFaker.Create().Generate();
        request.SetEmail("");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("must not be empty."));
    }

    [Fact]
    public async Task PostUserBanks_WhenEmailIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = PostUserBanksRequestFaker.Create().Generate();
        request.SetEmail("invalid-email");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("is not a valid email address."));
    }

    [Fact]
    public async Task PostUserBanks_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = PostUserBanksRequestFaker.Create().Generate();
        request.SetEmail("notfound@test.com");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserBanks(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.POSTUSERBANKSASYNC_CORESERVICE_404_USER_NOT_FOUND.GetDescription()));
    }

    [Fact]
    public async Task PostUserBanks_WhenBankNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = PostUserBanksRequestFaker.Create().Generate();
        request.SetEmail("test@test.com");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var user = UserEntityFaker.Create().Generate();
        user.Email = "test@test.com";
        authOrigin.Users.Add(user);
        await authOrigin.SaveChangesAsync();
        await DbContextMockFactory.SyncToAttachedDatabasesAsync(authOrigin);

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserBanks(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.POSTUSERBANKSASYNC_CORESERVICE_400_BANK_NOT_FOUND.GetDescription()));
    }

    [Fact]
    public async Task PostUserBanks_WhenUserBankAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var request = PostUserBanksRequestFaker.Create().Generate();
        request.SetEmail("test@test.com");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var user = UserEntityFaker.Create().Generate();
        user.Email = "test@test.com";
        authOrigin.Users.Add(user);
        await authOrigin.SaveChangesAsync();
        await DbContextMockFactory.SyncToAttachedDatabasesAsync(authOrigin);

        // Seed bank
        var existingBank = BankEntityFaker.Create().Generate();
        existingBank.Name = request.BankName;
        existingBank.BankType = request.BankType;
        origin.Bank.Add(existingBank);
        await origin.SaveChangesAsync();

        // Seed user bank
        var existingUserBank = UserBankEntityFaker.Create().Generate();
        existingUserBank.UserId = user.Id;
        existingUserBank.BankId = existingBank.Id;
        existingUserBank.NickName = request.NickName;
        origin.UserBank.Add(existingUserBank);
        await origin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.POSTUSERBANKSASYNC_CORESERVICE_400_ALREADY_EXISTS_USER_BANK.GetDescription()));
    }

    [Fact]
    public async Task PostUserBanks_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = PostUserBanksRequestFaker.Create().Generate();
        request.SetEmail("test@test.com");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed user
        var user = UserEntityFaker.Create().Generate();
        user.Email = "test@test.com";
        authOrigin.Users.Add(user);
        await authOrigin.SaveChangesAsync();
        await DbContextMockFactory.SyncToAttachedDatabasesAsync(authOrigin);

        // Seed bank
        var existingBank = BankEntityFaker.Create().Generate();
        existingBank.Name = request.BankName;
        existingBank.BankType = request.BankType;
        origin.Bank.Add(existingBank);
        await origin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserBanks(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().Be(EnumErrosApi.POSTUSERBANKSASYNC_CORESERVICE_200_CREATED.GetDescription());

        // Verify user bank was created
        var userBankInDb = await origin.UserBank.FirstOrDefaultAsync(ub => ub.UserId == user.Id && ub.BankId == existingBank.Id);
        userBankInDb.Should().NotBeNull();
        userBankInDb!.NickName.Should().Be(request.NickName);
    }
}
