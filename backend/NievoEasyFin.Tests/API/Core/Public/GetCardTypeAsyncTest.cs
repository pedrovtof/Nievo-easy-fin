using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using NievoEasyFin.Tests.Build.Request;

namespace NievoEasyFin.Tests.API.Core.Public;

public class GetCardTypeAsyncTest : AccountsServiceTestBase
{
    public GetCardTypeAsyncTest(ITestOutputHelper output) : base(output) { }

    [Fact(DisplayName = "GetCardType When Page Is Invalid Returns BadRequest")]
    public async Task GetCardType_WhenPageIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetCardTypeRequestBuilder { Page = 0 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetCardType(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page'."));
    }

    [Fact(DisplayName = "GetCardType When Page Size Is Invalid Returns BadRequest")]
    public async Task GetCardType_WhenPageSizeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetCardTypeRequestBuilder { PageSize = 0 };
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetCardType(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page Size'."));
    }

    [Fact(DisplayName = "GetCardType When No Card Types Exist Returns Ok With Empty List")]
    public async Task GetCardType_WhenNoCardTypesExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        var request = new GetCardTypeRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetCardType(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeAssignableTo<NievoEasyFin.Application.Interfaces.Response.ResponsePaginationBase<NievoEasyFin.Application.Data.Views.BankCardTypeView>>();
        var data = (NievoEasyFin.Application.Interfaces.Response.ResponsePaginationBase<NievoEasyFin.Application.Data.Views.BankCardTypeView>)response.Data;
        data.Items.Should().BeEmpty();
    }

    [Fact(DisplayName = "GetCardType With Valid Request Returns Ok With Data")]
    public async Task GetCardType_WithValidRequest_ReturnsOkWithData()
    {
        // Arrange
        var request = new GetCardTypeRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        
        origin.BankCardType.Add(BankCardTypeEntityFaker.Create().Generate());
        origin.BankCardType.Add(BankCardTypeEntityFaker.Create().Generate());
        await origin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetCardType(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();
    }
}
