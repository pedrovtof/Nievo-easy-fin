using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

public class GetPaymentTransactionTypeAsyncTest : AccountsServiceTestBase
{
    public GetPaymentTransactionTypeAsyncTest(ITestOutputHelper output) : base(output) { }

    #region BadRequest Errors

    [Fact(DisplayName = "GetPaymentTransactionType When Page Is Invalid Returns BadRequest")]
    public async Task GetPaymentTransactionType_WhenPageIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetPaymentTransactionTypeRequestBuilder().WithInvalidPage();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransactionType(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page'."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "GetPaymentTransactionType When PageSize Is Invalid Returns BadRequest")]
    public async Task GetPaymentTransactionType_WhenPageSizeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetPaymentTransactionTypeRequestBuilder().WithInvalidPageSize();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransactionType(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page Size'."));
        Output.WriteLine("Validation executed successfully.");
    }

    #endregion

    #region Success

    [Fact(DisplayName = "GetPaymentTransactionType When No Transaction Types Exist Returns Ok With Empty List")]
    public async Task GetPaymentTransactionType_WhenNoTransactionTypesExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        var request = new GetPaymentTransactionTypeRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransactionType(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeAssignableTo<ResponsePaginationBase<GetPaymentTransactionTypeResponse>>();

        var data = (ResponsePaginationBase<GetPaymentTransactionTypeResponse>)response.Data;
        data.Items.Should().BeEmpty();
        data.Records.Should().Be(0);
        Output.WriteLine("Success test executed correctly.");
    }

    [Fact(DisplayName = "GetPaymentTransactionType With Valid Request Returns Ok With Data")]
    public async Task GetPaymentTransactionType_WithValidRequest_ReturnsOkWithData()
    {
        // Arrange
        var request = new GetPaymentTransactionTypeRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var type1 = TransactionTypeEntityFaker.Create().Generate();
        var type2 = TransactionTypeEntityFaker.Create().Generate();
        origin.TransactionType.Add(type1);
        origin.TransactionType.Add(type2);

        await origin.SaveChangesAsync();
        await SyncCoreToAttachedDatabasesAsync(origin);

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransactionType(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeAssignableTo<ResponsePaginationBase<GetPaymentTransactionTypeResponse>>();

        var data = (ResponsePaginationBase<GetPaymentTransactionTypeResponse>)response.Data;
        data.Items.Should().NotBeEmpty();
        data.Items.Should().HaveCount(2);
        data.Records.Should().Be(2);
        Output.WriteLine("Success test executed correctly.");
    }

    #endregion
}
