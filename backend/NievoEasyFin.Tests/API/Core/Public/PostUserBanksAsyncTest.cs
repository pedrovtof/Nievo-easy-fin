using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit.Abstractions;
using NievoEasyFin.Tests.Build.Request;

namespace NievoEasyFin.Tests.API.Core.Public;

public class PostUserBanksAsyncTest : AccountsServiceTestBase
{
    public PostUserBanksAsyncTest(ITestOutputHelper output) : base(output) { }

    #region BadRequest Errors

    [Fact(DisplayName = "PostUserBanks When Bank Type Is Invalid Returns BadRequest")]
    public async Task PostUserBanks_WhenBankTypeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostUserBanksRequestBuilder();
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
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostUserBanks When Bank Name Is Empty Returns BadRequest")]
    public async Task PostUserBanks_WhenBankNameIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostUserBanksRequestBuilder();
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
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostUserBanks When Email Is Empty Returns BadRequest")]
    public async Task PostUserBanks_WhenEmailIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostUserBanksRequestBuilder();
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
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostUserBanks When Email Is Invalid Returns BadRequest")]
    public async Task PostUserBanks_WhenEmailIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostUserBanksRequestBuilder();
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
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostUserBanks When User Bank Already Exists Returns BadRequest")]
    public async Task PostUserBanks_WhenUserBankAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostUserBanksRequestBuilder();
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
        Output.WriteLine("Validation executed successfully.");
    }

    #endregion

    #region NotFound Errors

    [Fact(DisplayName = "PostUserBanks When User Not Found Returns NotFound")]
    public async Task PostUserBanks_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostUserBanksRequestBuilder();
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
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostUserBanks When Bank Not Found Returns NotFound")]
    public async Task PostUserBanks_WhenBankNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostUserBanksRequestBuilder();
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
        Output.WriteLine("Validation executed successfully.");
    }

    #endregion

    #region Success

    [Fact(DisplayName = "PostUserBanks With Valid Request Returns Created")]
    public async Task PostUserBanks_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = new PostUserBanksRequestBuilder();
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
        Output.WriteLine("Success test executed correctly.");
    }

    #endregion
}
