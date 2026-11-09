using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

public class GetPaymentTransactionAsyncTest : AccountsServiceTestBase
{
    public GetPaymentTransactionAsyncTest(ITestOutputHelper output) : base(output) { }

    #region BadRequest Errors

    [Fact(DisplayName = "GetPaymentTransaction When Page Is Invalid Returns BadRequest")]
    public async Task GetPaymentTransaction_WhenPageIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetPaymentTransactionRequestBuilder().WithInvalidPage();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page'."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "GetPaymentTransaction When PageSize Is Invalid Returns BadRequest")]
    public async Task GetPaymentTransaction_WhenPageSizeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetPaymentTransactionRequestBuilder().WithInvalidPageSize();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page Size'."));
        Output.WriteLine("Validation executed successfully.");
    }

    #endregion

    #region NotFound Errors

    [Fact(DisplayName = "GetPaymentTransaction When User Not Found Returns NotFound")]
    public async Task GetPaymentTransaction_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new GetPaymentTransactionRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.GETPAYMENTTRANSACTION_CORESERVICE_404_USER_NOT_FOUND.GetDescription());
        Output.WriteLine("Validation executed successfully.");
    }

    #endregion

    #region Success

    [Fact(DisplayName = "GetPaymentTransaction With Valid Request Returns Ok")]
    public async Task GetPaymentTransaction_WithValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new GetPaymentTransactionRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeAssignableTo<ResponsePaginationBase<GetPaymentTransactionAsyncResponse>>();

        var data = (ResponsePaginationBase<GetPaymentTransactionAsyncResponse>)response.Data;
        data.Items.Should().BeEmpty();
        data.Records.Should().Be(0);
        Output.WriteLine("Success test executed correctly.");
    }

    #endregion
}
