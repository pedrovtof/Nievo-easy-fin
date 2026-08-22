using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit.Abstractions;
using NievoEasyFin.Tests.Build.Request;

namespace NievoEasyFin.Tests.API.Core.Public;

public class GetCardFlagAsyncTest : AccountsServiceTestBase
{
    public GetCardFlagAsyncTest(ITestOutputHelper output) : base(output) { }

    [Fact(DisplayName = "GetCardFlag When Page Is Invalid Returns BadRequest")]
    public async Task GetCardFlag_WhenPageIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetCardFlagRequestBuilder { Page = 0 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetCardFlag(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page'."));
    }

    [Fact(DisplayName = "GetCardFlag When PageSize Is Invalid Returns BadRequest")]
    public async Task GetCardFlag_WhenPageSizeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetCardFlagRequestBuilder { PageSize = 0 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetCardFlag(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page Size'."));
    }

    [Fact(DisplayName = "GetCardFlag With Valid Request Returns Ok")]
    public async Task GetCardFlag_WithValidRequest_ReturnsOkWithData()
    {
        // Arrange
        var request = new GetCardFlagRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var flag1 = BankCardFlagEntityFaker.Create().Generate();
        var flag2 = BankCardFlagEntityFaker.Create().Generate();
        origin.BankCardFlag.Add(flag1);
        origin.BankCardFlag.Add(flag2);
        
        await origin.SaveChangesAsync();
        await SyncCoreToAttachedDatabasesAsync(origin);

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetCardFlag(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();
        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeAssignableTo<ResponsePaginationBase<NievoEasyFin.Application.Data.Views.BankCardFlagView>>();
        var data = (ResponsePaginationBase<NievoEasyFin.Application.Data.Views.BankCardFlagView>)response.Data;
        data.Items.Should().NotBeEmpty();
    }
}
