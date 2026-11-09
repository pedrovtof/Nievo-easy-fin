using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

public class GetPaymentTransactionStatusAsyncTest : AccountsServiceTestBase
{
    public GetPaymentTransactionStatusAsyncTest(ITestOutputHelper output) : base(output) { }

    #region BadRequest Errors

    [Fact(DisplayName = "GetPaymentTransactionStatus When Page Is Invalid Returns BadRequest")]
    public async Task GetPaymentTransactionStatus_WhenPageIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetPaymentTransactionStatusRequestBuilder().WithInvalidPage();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransactionStatus(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page'."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "GetPaymentTransactionStatus When PageSize Is Invalid Returns BadRequest")]
    public async Task GetPaymentTransactionStatus_WhenPageSizeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetPaymentTransactionStatusRequestBuilder().WithInvalidPageSize();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransactionStatus(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page Size'."));
        Output.WriteLine("Validation executed successfully.");
    }

    #endregion

    #region Success

    [Fact(DisplayName = "GetPaymentTransactionStatus When No Transaction Status Exist Returns Ok With Empty List")]
    public async Task GetPaymentTransactionStatus_WhenNoTransactionStatusExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        var request = new GetPaymentTransactionStatusRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransactionStatus(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeAssignableTo<ResponsePaginationBase<GetPaymentTransactionStatusResponse>>();

        var data = (ResponsePaginationBase<GetPaymentTransactionStatusResponse>)response.Data;
        data.Items.Should().BeEmpty();
        data.Records.Should().Be(0);
        Output.WriteLine("Success test executed correctly.");
    }

    [Fact(DisplayName = "GetPaymentTransactionStatus With Valid Request Returns Ok With Data")]
    public async Task GetPaymentTransactionStatus_WithValidRequest_ReturnsOkWithData()
    {
        // Arrange
        var request = new GetPaymentTransactionStatusRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var status1 = TransactionStatusEntityFaker.Create().Generate();
        var status2 = TransactionStatusEntityFaker.Create().Generate();
        origin.TransactionStatus.Add(status1);
        origin.TransactionStatus.Add(status2);

        await origin.SaveChangesAsync();
        await SyncCoreToAttachedDatabasesAsync(origin);

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransactionStatus(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeAssignableTo<ResponsePaginationBase<GetPaymentTransactionStatusResponse>>();

        var data = (ResponsePaginationBase<GetPaymentTransactionStatusResponse>)response.Data;
        data.Items.Should().NotBeEmpty();
        data.Items.Should().HaveCount(2);
        data.Records.Should().Be(2);
        Output.WriteLine("Success test executed correctly.");
    }

    #endregion
}
