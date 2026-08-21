using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit.Abstractions;
using NievoEasyFin.Tests.Build.Request;

namespace NievoEasyFin.Tests.API.Core.Public;

public class GetBankCardAsyncTest : AccountsServiceTestBase
{
    public GetBankCardAsyncTest(ITestOutputHelper output) : base(output) { }

    #region BadRequest Errors

    [Fact(DisplayName = "GetBankCard When Page Is Invalid Returns BadRequest")]
    public async Task GetBankCard_WhenPageIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetBankCardRequestBuilder { Page = 0 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBankCard(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page'.") || e.Contains("GETBANKCARDASYNC_CORESERVICE_400_INVALID_PAGE"));
    }

    [Fact(DisplayName = "GetBankCard When Page Size Is Invalid Returns BadRequest")]
    public async Task GetBankCard_WhenPageSizeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetBankCardRequestBuilder { PageSize = 0 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBankCard(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page Size'.") || e.Contains("GETBANKCARDASYNC_CORESERVICE_400_INVALID_PAGE"));
    }

    [Fact(DisplayName = "GetBankCard When Email Is Empty Returns BadRequest")]
    public async Task GetBankCard_WhenEmailIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetBankCardRequestBuilder();
        request.SetEmail(string.Empty);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBankCard(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("must not be empty"));
    }

    [Fact(DisplayName = "GetBankCard When Email Is Invalid Returns BadRequest")]
    public async Task GetBankCard_WhenEmailIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetBankCardRequestBuilder().WithInvalidEmail();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBankCard(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("not a valid email address"));
    }

    [Fact(DisplayName = "GetBankCard When BankId Is Invalid Returns BadRequest")]
    public async Task GetBankCard_WhenBankIdIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetBankCardRequestBuilder { BankId = 0 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBankCard(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Bank Id'."));
    }

    [Fact(DisplayName = "GetBankCard When CardType Is Invalid Returns BadRequest")]
    public async Task GetBankCard_WhenCardTypeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetBankCardRequestBuilder { CardType = 0 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBankCard(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Card Type'."));
    }

    #endregion

    #region Success

    [Fact(DisplayName = "GetBankCard When No Cards Exist Returns Ok With Empty List")]
    public async Task GetBankCard_WhenNoCardsExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        var request = new GetBankCardRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBankCard(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        var response = (ResponseApiSucess)okResult.Value!;

        response.Data.Should().BeAssignableTo<ResponsePaginationBase<NievoEasyFin.Application.Data.Views.BankCardView>>();
        var data = (ResponsePaginationBase<NievoEasyFin.Application.Data.Views.BankCardView>)response.Data;
        data.Items.Should().BeEmpty();
    }

    [Fact(DisplayName = "GetBankCard With Valid Request Returns Ok With Data")]
    public async Task GetBankCard_WithValidRequest_ReturnsOkWithData()
    {
        // Arrange
        var request = new GetBankCardRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var bank = BankEntityFaker.Create().Generate();
        var cardType = BankCardTypeEntityFaker.Create().Generate();

        var card1 = BankCardEntityFaker.Create().Generate();
        card1.BankId = bank.Id;
        card1.CardType = cardType.Id;

        var card2 = BankCardEntityFaker.Create().Generate();
        card2.BankId = bank.Id;
        card2.CardType = cardType.Id;

        origin.BankCardType.Add(cardType);
        origin.Bank.Add(bank);
        origin.BankCard.Add(card1);
        origin.BankCard.Add(card2);

        await origin.SaveChangesAsync();
        await SyncCoreToAttachedDatabasesAsync(origin);

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBankCard(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();
        var response = (ResponseApiSucess)okResult.Value!;

        response.Data.Should().BeAssignableTo<ResponsePaginationBase<NievoEasyFin.Application.Data.Views.BankCardView>>();
        var data = (ResponsePaginationBase<NievoEasyFin.Application.Data.Views.BankCardView>)response.Data;
        data.Items.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "GetBankCard With Filters Returns Filtered Data")]
    public async Task GetBankCard_WithFilters_ReturnsFilteredData()
    {
        // Arrange
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var bank = BankEntityFaker.Create().Generate();
        var cardType = BankCardTypeEntityFaker.Create().Generate();

        var card1 = BankCardEntityFaker.Create().Generate();
        card1.BankId = bank.Id;
        card1.CardType = cardType.Id;

        var card2 = BankCardEntityFaker.Create().Generate(); // Not matching filters

        origin.BankCardType.Add(cardType);
        origin.Bank.Add(bank);
        origin.BankCard.Add(card1);
        origin.BankCard.Add(card2);

        await origin.SaveChangesAsync();
        await SyncCoreToAttachedDatabasesAsync(origin);

        var request = new GetBankCardRequestBuilder
        {
            BankId = bank.Id,
            CardType = cardType.Id
        };
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBankCard(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();
        var response = (ResponseApiSucess)okResult.Value!;

        response.Data.Should().BeAssignableTo<ResponsePaginationBase<NievoEasyFin.Application.Data.Views.BankCardView>>();
        var data = (ResponsePaginationBase<NievoEasyFin.Application.Data.Views.BankCardView>)response.Data;
        data.Items.Should().NotBeEmpty();
        data.Items.Should().OnlyContain(x => x.Bank == bank.Name && x.CardType == cardType.Name);
    }

    #endregion
}
