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

public class BankControllerPublicTest
{
    private readonly IPublicBankService _mockBankService;
    private readonly JsonWebTokenService _jwtService;
    private readonly BankController _controller;
    private readonly ITestOutputHelper _output;

    public BankControllerPublicTest(ITestOutputHelper output)
    {
        _output = output;
        _mockBankService = Substitute.For<IPublicBankService>();
        _jwtService = new JsonWebTokenService(null!);
        _controller = new BankController(_mockBankService, _jwtService);
    }

    [Fact(DisplayName = "GetBanks calls IPublicBankService and returns Ok")]
    public async Task GetBanks_ValidRequest_CallsServiceAndReturnsResult()
    {
        // Arrange
        var expected = new OkObjectResult(new ResponseApiSucess("ok"));
        _mockBankService.GetBanks(Arg.Any<GetBanksRequest>())
            .Returns(Task.FromResult<IActionResult>(expected));

        var request = new GetBanksRequest { Page = 1, PageSize = 10 };

        // Act
        var result = await _controller.GetBanks(request);

        // Assert
        result.Should().BeSameAs(expected);
        await _mockBankService.Received(1).GetBanks(request);
    }

    [Fact(DisplayName = "GetBankCard calls IPublicBankService and returns Ok")]
    public async Task GetBankCard_ValidRequest_CallsServiceAndReturnsResult()
    {
        // Arrange
        var expected = new OkObjectResult(new ResponseApiSucess("ok"));
        _mockBankService.GetBankCard(Arg.Any<GetBankCardRequest>())
            .Returns(Task.FromResult<IActionResult>(expected));

        var request = new GetBankCardRequest { BankId = 1, Page = 1, PageSize = 10 };

        // Act
        var result = await _controller.GetBankCard("fake-token", request);

        // Assert
        result.Should().BeSameAs(expected);
        await _mockBankService.Received(1).GetBankCard(request);
    }
}
