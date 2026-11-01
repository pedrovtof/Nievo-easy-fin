using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NievoEasyFin.Tests.Mocks.Database;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

public class PostPaymentTransactionAsyncTest : AccountsServiceTestBase
{
    public PostPaymentTransactionAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "PostPaymentTransaction With Valid Request Returns Ok")]
    public async Task PostPaymentTransaction_WithValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();
        Output.WriteLine("Success test executed correctly.");
    }

    #endregion
}
