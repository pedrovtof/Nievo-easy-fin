using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Services.Security;
using NievoEasyFin.Core.Controllers.Private;
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Private;

public class BankControllerPrivateTest
{
    private readonly IPrivateBankService _mockBankService;
    private readonly JsonWebTokenService _jwtService;
    private readonly BankController _controller;
    private readonly ITestOutputHelper _output;

    public BankControllerPrivateTest(ITestOutputHelper output)
    {
        _output = output;
        _mockBankService = Substitute.For<IPrivateBankService>();
        _jwtService = new JsonWebTokenService(null!);
        _controller = new BankController(_mockBankService, _jwtService);
    }

    [Fact(DisplayName = "PostAccountsBanks calls IPrivateBankService and returns Ok")]
    public async Task PostAccountsBanks_ValidRequest_CallsServiceAndReturnsResult()
    {
        // Arrange
        var expected = new OkObjectResult(new ResponseApiSucess("ok"));
        _mockBankService.PostAccountsBanks(Arg.Any<PostAccountsBanksRequest>())
            .Returns(Task.FromResult<IActionResult>(expected));

        var request = new PostAccountsBanksRequest { Name = "TestBank", BankType = 1 };

        // Act
        var result = await _controller.PostAccountsBanks(request);

        // Assert
        result.Should().BeSameAs(expected);
        await _mockBankService.Received(1).PostAccountsBanks(request);
    }

    [Fact(DisplayName = "PostBankCard calls IPrivateBankService and returns Ok")]
    public async Task PostBankCard_ValidRequest_CallsServiceAndReturnsResult()
    {
        // Arrange
        var expected = new OkObjectResult(new ResponseApiSucess("ok"));
        _mockBankService.PostBankCard(Arg.Any<PostBankCardRequest>())
            .Returns(Task.FromResult<IActionResult>(expected));

        var request = new PostBankCardRequest { BankId = 1, CardType = 1, Name = "Gold", Flag = "Visa" };

        // Act
        var result = await _controller.PostBankCard(request);

        // Assert
        result.Should().BeSameAs(expected);
        await _mockBankService.Received(1).PostBankCard(request);
    }
}
