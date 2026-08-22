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

public class PostBankCardAsyncTest : AccountsServiceTestBase
{
    public PostBankCardAsyncTest(ITestOutputHelper output) : base(output) { }

    [Fact(DisplayName = "PostBankCard When Name Is Empty Returns BadRequest")]
    public async Task PostBankCard_WhenNameIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostBankCardRequestBuilder { Name = "" };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostBankCard(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Name' must not be empty."));
    }

    [Fact(DisplayName = "PostBankCard When BankId Is Invalid Returns BadRequest")]
    public async Task PostBankCard_WhenBankIdIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostBankCardRequestBuilder { BankId = 0 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostBankCard(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Bank Id' must be greater than or equal to '1'."));
    }

    [Fact(DisplayName = "PostBankCard When CardType Is Invalid Returns BadRequest")]
    public async Task PostBankCard_WhenCardTypeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostBankCardRequestBuilder { CardType = 0 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostBankCard(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Card Type' must be greater than or equal to '1'."));
    }

    [Fact(DisplayName = "PostBankCard When Bank Not Found Returns NotFound")]
    public async Task PostBankCard_WhenBankNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostBankCardRequestBuilder { BankId = 999 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostBankCard(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_404_BANK_NOT_FOUND.GetDescription()));
    }

    [Fact(DisplayName = "PostBankCard When Card Type Not Found Returns NotFound")]
    public async Task PostBankCard_WhenCardTypeNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostBankCardRequestBuilder { BankId = 1, CardType = 999 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingBank = BankEntityFaker.Create().Generate();
        existingBank.Id = 1;
        origin.Bank.Add(existingBank);
        await origin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostBankCard(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_404_CARD_TYPE_NOT_FOUND.GetDescription()));
    }

    [Fact(DisplayName = "PostBankCard When Card Already Exists Returns BadRequest")]
    public async Task PostBankCard_WhenCardAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostBankCardRequestBuilder { BankId = 1, CardType = 1, Name = "Black Card" };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingBank = BankEntityFaker.Create().Generate();
        existingBank.Id = 1;
        origin.Bank.Add(existingBank);

        var existingCardType = BankCardTypeEntityFaker.Create().Generate();
        existingCardType.Id = 1;
        origin.BankCardType.Add(existingCardType);

        var existingCard = BankCardEntityFaker.Create().Generate();
        existingCard.BankId = 1;
        existingCard.CardType = 1;
        existingCard.Name = "Black Card";
        origin.BankCard.Add(existingCard);

        await origin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostBankCard(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_400_CARD_ALREADY_EXISTS.GetDescription()));
    }

    [Fact(DisplayName = "PostBankCard With Valid Request Returns Created")]
    public async Task PostBankCard_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = new PostBankCardRequestBuilder { Name = "New Card", BankId = 1, CardType = 1 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingBank = BankEntityFaker.Create().Generate();
        existingBank.Id = 1;
        origin.Bank.Add(existingBank);

        var existingCardType = BankCardTypeEntityFaker.Create().Generate();
        existingCardType.Id = 1;
        origin.BankCardType.Add(existingCardType);
        var existingFlag = BankCardFlagEntityFaker.Create().Generate();
        existingFlag.Name = "Mastercard";
        origin.BankCardFlag.Add(existingFlag);
        await origin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostBankCard(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();
        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().Be(EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_200_CARD_CREATED.GetDescription());

        var cardInDb = await origin.BankCard.FirstOrDefaultAsync(bc => bc.BankId == 1 && bc.CardType == 1 && bc.Name == "New Card");
        cardInDb.Should().NotBeNull();
    }
}
