using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Services.Base;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

public class PaymentServiceDirectTest
{
    private readonly ITestOutputHelper _output;

    public PaymentServiceDirectTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact(DisplayName = "PostPaymentTransaction directly on PaymentService returns Ok")]
    public async Task PostPaymentTransaction_DirectService_ReturnsOk()
    {
        // Arrange
        var service = new PaymentService();
        var request = new PostPaymentTransactionRequest();

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();
    }
}
