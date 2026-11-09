using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Services.Security;
using NievoEasyFin.Core.Controllers.Public;
using NievoEasyFin.Tests.Build.Request;
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

    [Fact(DisplayName = "GetPaymentTransactionStatus calls IPaymentService and returns Ok")]
    public async Task GetPaymentTransactionStatus_ValidRequest_CallsServiceAndReturnsResult()
    {
        // Arrange
        var expected = new OkObjectResult(new ResponseApiSucess("ok"));
        _mockPaymentService.GetPaymentTransactionStatus(Arg.Any<GetPaymentTransactionStatusRequest>())
            .Returns(Task.FromResult<IActionResult>(expected));

        var request = new GetPaymentTransactionStatusRequestBuilder();

        // Act
        var result = await _controller.GetPaymentTransactionStatus("fake-token", request);

        // Assert
        result.Should().BeSameAs(expected);
        await _mockPaymentService.Received(1).GetPaymentTransactionStatus(request);
    }

    [Fact(DisplayName = "GetPaymentTransactionType calls IPaymentService and returns Ok")]
    public async Task GetPaymentTransactionType_ValidRequest_CallsServiceAndReturnsResult()
    {
        // Arrange
        var expected = new OkObjectResult(new ResponseApiSucess("ok"));
        _mockPaymentService.GetPaymentTransactionType(Arg.Any<GetPaymentTransactionTypeRequest>())
            .Returns(Task.FromResult<IActionResult>(expected));

        var request = new GetPaymentTransactionTypeRequestBuilder();

        // Act
        var result = await _controller.GetPaymentTransactionType("fake-token", request);

        // Assert
        result.Should().BeSameAs(expected);
        await _mockPaymentService.Received(1).GetPaymentTransactionType(request);
    }

    [Fact(DisplayName = "GetPaymentScheduler calls IPaymentService and returns Ok")]
    public async Task GetPaymentScheduler_ValidRequest_CallsServiceAndReturnsResult()
    {
        // Arrange
        var expected = new OkObjectResult(new ResponseApiSucess("ok"));
        _mockPaymentService.GetPaymentScheduler(Arg.Any<GetPaymentSchedulerRequest>())
            .Returns(Task.FromResult<IActionResult>(expected));

        var request = new GetPaymentSchedulerRequestBuilder();

        // Act
        var result = await _controller.GetPaymentScheduler("fake-token", request);

        // Assert
        result.Should().BeSameAs(expected);
        await _mockPaymentService.Received(1).GetPaymentScheduler(request);
    }

    [Fact(DisplayName = "PostPaymentTransaction calls IPaymentService and returns Ok")]
    public async Task PostPaymentTransaction_ValidRequest_CallsServiceAndReturnsResult()
    {
        // Arrange
        var expected = new OkObjectResult(new ResponseApiSucess("ok"));
        _mockPaymentService.PostPaymentTransaction(Arg.Any<PostPaymentTransactionRequest>())
            .Returns(Task.FromResult<IActionResult>(expected));

        var request = new PostPaymentTransactionRequestBuilder();

        // Act
        var result = await _controller.PostPaymentTransaction("fake-token", request);

        // Assert
        result.Should().BeSameAs(expected);
        await _mockPaymentService.Received(1).PostPaymentTransaction(request);
    }

    [Fact(DisplayName = "GetPaymentTransaction calls IPaymentService and returns Ok")]
    public async Task GetPaymentTransaction_ValidRequest_CallsServiceAndReturnsResult()
    {
        // Arrange
        var expected = new OkObjectResult(new ResponseApiSucess("ok"));
        _mockPaymentService.GetPaymentTransaction(Arg.Any<GetPaymentTransactionRequest>())
            .Returns(Task.FromResult<IActionResult>(expected));

        var request = new GetPaymentTransactionRequestBuilder();

        // Act
        var result = await _controller.GetPaymentTransaction("fake-token", request);

        // Assert
        result.Should().BeSameAs(expected);
        await _mockPaymentService.Received(1).GetPaymentTransaction(request);
    }
}
