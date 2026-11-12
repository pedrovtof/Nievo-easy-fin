using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

/// <summary>
/// Unit and integration tests for <see cref="NievoEasyFin.Application.Services.Base.PaymentService.GetPaymentTransactionSituation"/>.
/// </summary>
public class GetPaymentTransactionSituationAsyncTest : AccountsServiceTestBase
{
    public GetPaymentTransactionSituationAsyncTest(ITestOutputHelper output) : base(output) { }

    #region BadRequest Errors

    [Fact(DisplayName = "GetPaymentTransactionSituation When Page Is Invalid Returns BadRequest")]
    public async Task GetPaymentTransactionSituation_WhenPageIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetPaymentTransactionSituationRequestBuilder().WithInvalidPage();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransactionSituation(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page'."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "GetPaymentTransactionSituation When PageSize Is Invalid Returns BadRequest")]
    public async Task GetPaymentTransactionSituation_WhenPageSizeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetPaymentTransactionSituationRequestBuilder().WithInvalidPageSize();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransactionSituation(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("The specified condition was not met for 'Page Size'."));
        Output.WriteLine("Validation executed successfully.");
    }

    #endregion

    #region Success

    [Fact(DisplayName = "GetPaymentTransactionSituation When No Situation Exists Returns Ok With Empty List")]
    public async Task GetPaymentTransactionSituation_WhenNoSituationExists_ReturnsOkWithEmptyList()
    {
        // Arrange
        var request = new GetPaymentTransactionSituationRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransactionSituation(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeAssignableTo<ResponsePaginationBase<GetPaymentTransactionSituationAsyncResponse>>();

        var data = (ResponsePaginationBase<GetPaymentTransactionSituationAsyncResponse>)response.Data;
        data.Items.Should().BeEmpty();
        data.Records.Should().Be(0);
        Output.WriteLine("Success test executed correctly.");
    }

    [Fact(DisplayName = "GetPaymentTransactionSituation With Valid Request Returns Ok With Data")]
    public async Task GetPaymentTransactionSituation_WithValidRequest_ReturnsOkWithData()
    {
        // Arrange
        var request = new GetPaymentTransactionSituationRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var sit1 = TransactionSituationEntityFaker.Create().Generate();
        sit1.Name = "Entrada";
        var sit2 = TransactionSituationEntityFaker.Create().Generate();
        sit2.Name = "Saida";
        origin.TransactionSituation.Add(sit1);
        origin.TransactionSituation.Add(sit2);

        await origin.SaveChangesAsync();
        await SyncCoreToAttachedDatabasesAsync(origin);

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetPaymentTransactionSituation(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeAssignableTo<ResponsePaginationBase<GetPaymentTransactionSituationAsyncResponse>>();

        var data = (ResponsePaginationBase<GetPaymentTransactionSituationAsyncResponse>)response.Data;
        data.Items.Should().NotBeEmpty();
        data.Items.Should().HaveCount(2);
        data.Records.Should().Be(2);
        data.Items.Select(x => x.Name).Should().Contain(new[] { "Entrada", "Saida" });
        Output.WriteLine("Success test executed correctly.");
    }

    #endregion
}
