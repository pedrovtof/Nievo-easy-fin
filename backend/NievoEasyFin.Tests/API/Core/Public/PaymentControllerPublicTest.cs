using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Services.Security;
using NievoEasyFin.Core.Controllers.Public;
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

public class PaymentControllerPublicTest
{
    private readonly IPaymentService _mockPaymentService;
    private readonly JsonWebTokenService _jwtService;
    private readonly PaymentController _controller;
    private readonly ITestOutputHelper _output;

    public PaymentControllerPublicTest(ITestOutputHelper output)
    {
        _output = output;
        _mockPaymentService = Substitute.For<IPaymentService>();
        _jwtService = new JsonWebTokenService(null!);
        _controller = new PaymentController(_mockPaymentService, _jwtService);
    }

    [Fact(DisplayName = "PostPaymentTransaction calls IPaymentService and returns Ok")]
    public async Task PostPaymentTransaction_ValidRequest_CallsServiceAndReturnsResult()
    {
        // Arrange
        var expected = new OkObjectResult(new ResponseApiSucess("ok"));
        _mockPaymentService.PostPaymentTransaction(Arg.Any<PostPaymentTransactionRequest>())
            .Returns(Task.FromResult<IActionResult>(expected));

        var request = new PostPaymentTransactionRequest();

        // Act
        var result = await _controller.PostPaymentTransaction("Bearer fake-token", request);

        // Assert
        result.Should().BeSameAs(expected);
        await _mockPaymentService.Received(1).PostPaymentTransaction(request);
    }
}
