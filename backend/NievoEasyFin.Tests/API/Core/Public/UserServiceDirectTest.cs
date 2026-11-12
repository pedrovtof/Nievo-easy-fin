using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

/// <summary>
/// Direct service integration tests for <see cref="NievoEasyFin.Application.Services.Base.CoreUserService"/> using in-memory SQLite contexts.
/// </summary>
public class UserServiceDirectTest : AccountsServiceTestBase
{
    /// <summary>
    /// Initializes a new instance of <see cref="UserServiceDirectTest"/>.
    /// </summary>
    /// <param name="output">Test output helper.</param>
    public UserServiceDirectTest(ITestOutputHelper output) : base(output) { }

    /// <summary>
    /// Verifies that GetUserBanks returns BadRequest when email validation fails.
    /// </summary>
    [Fact(DisplayName = "GetUserBanks directly on UserService with invalid page returns BadRequest")]
    public async Task GetUserBanks_DirectUserService_InvalidPage_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetUserBanksRequestBuilder().WithEmail("");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateUserService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetUserBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().NotBeEmpty();
    }

    /// <summary>
    /// Verifies that PostUserCard returns BadRequest when request payload validation fails.
    /// </summary>
    [Fact(DisplayName = "PostUserCard directly on UserService with invalid request returns BadRequest")]
    public async Task PostUserCard_WhenValidationFails_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostUserCardRequestBuilder().WithEmail("");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateUserService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserCard(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().NotBeEmpty();
    }

    /// <summary>
    /// Verifies that PostUserCard returns NotFound when the target bank does not exist.
    /// </summary>
    [Fact(DisplayName = "PostUserCard When Bank Not Found Returns NotFound")]
    public async Task PostUserCard_WhenBankNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostUserCardRequestBuilder().WithBankId(999);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateUserService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserCard(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_404_BANK_NOT_FOUND.GetDescription());
    }

    /// <summary>
    /// Verifies that PostUserCard returns NotFound when the bank card does not exist for the given bank.
    /// </summary>
    [Fact(DisplayName = "PostUserCard When BankCard Not Found Returns NotFound")]
    public async Task PostUserCard_WhenBankCardNotFound_ReturnsNotFound()
    {
        // Arrange
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingBank = BankEntityFaker.Create().Generate();
        origin.Bank.Add(existingBank);
        await origin.SaveChangesAsync();

        var request = new PostUserCardRequestBuilder()
            .WithBankId(existingBank.Id)
            .WithCardId(999);

        var service = CreateUserService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserCard(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_404_BANKCARD_NOT_FOUND.GetDescription());
    }

    /// <summary>
    /// Verifies that PostUserCard returns NotFound when the authenticated user is not found in the database.
    /// </summary>
    [Fact(DisplayName = "PostUserCard When User Not Found Returns NotFound")]
    public async Task PostUserCard_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingBank = BankEntityFaker.Create().Generate();
        origin.Bank.Add(existingBank);
        await origin.SaveChangesAsync();

        var existingCard = BankCardEntityFaker.Create().Generate();
        existingCard.BankId = existingBank.Id;
        existingCard.Active = true;
        origin.BankCard.Add(existingCard);
        await origin.SaveChangesAsync();

        var request = new PostUserCardRequestBuilder()
            .WithEmail("notfound@test.com")
            .WithBankId(existingBank.Id)
            .WithCardId(existingCard.Id);

        var service = CreateUserService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserCard(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_404_USER_NOT_FOUND.GetDescription());
    }

    /// <summary>
    /// Verifies that PostUserCard returns BadRequest when an active card already exists for the same card_id and user_id (idx_unic_user_bank_card).
    /// </summary>
    [Fact(DisplayName = "PostUserCard When User Card Already Exists (card_id + user_id) Returns BadRequest")]
    public async Task PostUserCard_WhenUserCardAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = "usercard@test.com";
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var existingBank = BankEntityFaker.Create().Generate();
        origin.Bank.Add(existingBank);
        await origin.SaveChangesAsync();

        var existingCard = BankCardEntityFaker.Create().Generate();
        existingCard.BankId = existingBank.Id;
        existingCard.Active = true;
        origin.BankCard.Add(existingCard);
        await origin.SaveChangesAsync();

        var existingUserBankCard = UserBankCardEntityFaker.Create().Generate();
        existingUserBankCard.BankId = existingBank.Id;
        existingUserBankCard.CardId = existingCard.Id;
        existingUserBankCard.UserId = existingUser.Id;
        existingUserBankCard.Active = true;
        origin.UserBankCard.Add(existingUserBankCard);
        await origin.SaveChangesAsync();

        var request = new PostUserCardRequestBuilder()
            .WithEmail(existingUser.Email)
            .WithBankId(existingBank.Id)
            .WithCardId(existingCard.Id);

        var service = CreateUserService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserCard(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_USER_CARD_ALREADY_EXISTS.GetDescription());
    }

    /// <summary>
    /// Verifies that PostUserCard creates the user bank card record and returns Ok when all validations pass.
    /// </summary>
    [Fact(DisplayName = "PostUserCard With Valid Request Creates UserBankCard And Returns Ok")]
    public async Task PostUserCard_WithValidRequest_ReturnsOk()
    {
        // Arrange
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = "validusercard@test.com";
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var existingBank = BankEntityFaker.Create().Generate();
        origin.Bank.Add(existingBank);
        await origin.SaveChangesAsync();

        var existingCard = BankCardEntityFaker.Create().Generate();
        existingCard.BankId = existingBank.Id;
        existingCard.Active = true;
        origin.BankCard.Add(existingCard);
        await origin.SaveChangesAsync();

        var request = new PostUserCardRequestBuilder()
            .WithEmail(existingUser.Email)
            .WithBankId(existingBank.Id)
            .WithCardId(existingCard.Id);

        var service = CreateUserService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostUserCard(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().Be(EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_200_CARD_CREATED.GetDescription());

        var createdInDb = await origin.UserBankCard.FirstOrDefaultAsync(x => x.CardId == existingCard.Id && x.UserId == existingUser.Id);
        createdInDb.Should().NotBeNull();
        createdInDb!.Name.Should().Be(request.CardUserName);
    }
}
