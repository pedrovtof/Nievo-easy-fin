using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

public class GetPaymentSchedulerAsyncTest : AccountsServiceTestBase
{
    public GetPaymentSchedulerAsyncTest(ITestOutputHelper output) : base(output) { }

    #region BadRequest Errors

    [Fact(DisplayName = "GetPaymentScheduler When Page Is Invalid Returns BadRequest")]
    public async Task GetPaymentScheduler_WhenPageIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetPaymentSchedulerRequestBuilder().WithInvalidPage();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentScheduler(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page'."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "GetPaymentScheduler When PageSize Is Invalid Returns BadRequest")]
    public async Task GetPaymentScheduler_WhenPageSizeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetPaymentSchedulerRequestBuilder().WithInvalidPageSize();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentScheduler(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page Size'."));
        Output.WriteLine("Validation executed successfully.");
    }

    #endregion

    #region Success

    [Fact(DisplayName = "GetPaymentScheduler When No Schedulers Exist Returns Ok With Empty List")]
    public async Task GetPaymentScheduler_WhenNoSchedulersExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        var request = new GetPaymentSchedulerRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentScheduler(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeAssignableTo<ResponsePaginationBase<GetSchedulerTypeAsyncResponse>>();

        var data = (ResponsePaginationBase<GetSchedulerTypeAsyncResponse>)response.Data;
        data.Items.Should().BeEmpty();
        data.Records.Should().Be(0);
        Output.WriteLine("Success test executed correctly.");
    }

    [Fact(DisplayName = "GetPaymentScheduler With Valid Request Returns Ok With Data")]
    public async Task GetPaymentScheduler_WithValidRequest_ReturnsOkWithData()
    {
        // Arrange
        var request = new GetPaymentSchedulerRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var scheduler1 = SchedulerTypeEntityFaker.Create().Generate();
        var scheduler2 = SchedulerTypeEntityFaker.Create().Generate();
        origin.SchedulerType.Add(scheduler1);
        origin.SchedulerType.Add(scheduler2);

        await origin.SaveChangesAsync();
        await SyncCoreToAttachedDatabasesAsync(origin);

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentScheduler(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeAssignableTo<ResponsePaginationBase<GetSchedulerTypeAsyncResponse>>();

        var data = (ResponsePaginationBase<GetSchedulerTypeAsyncResponse>)response.Data;
        data.Items.Should().NotBeEmpty();
        data.Items.Should().HaveCount(2);
        data.Records.Should().Be(2);
        Output.WriteLine("Success test executed correctly.");
    }

    #endregion
}
