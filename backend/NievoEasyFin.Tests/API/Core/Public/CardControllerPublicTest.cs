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

public class CardControllerPublicTest
{
    private readonly ICardService _mockCardService;
    private readonly JsonWebTokenService _jwtService;
    private readonly CardController _controller;
    private readonly ITestOutputHelper _output;

    public CardControllerPublicTest(ITestOutputHelper output)
    {
        _output = output;
        _mockCardService = Substitute.For<ICardService>();
        _jwtService = new JsonWebTokenService(null!);
        _controller = new CardController(_mockCardService, _jwtService);
    }

    [Fact(DisplayName = "GetCardType calls ICardService and returns Ok")]
    public async Task GetCardType_ValidRequest_CallsServiceAndReturnsResult()
    {
        // Arrange
        var expected = new OkObjectResult(new ResponseApiSucess("ok"));
        _mockCardService.GetCardType(Arg.Any<GetCardTypeRequest>())
            .Returns(Task.FromResult<IActionResult>(expected));

        var request = new GetCardTypeRequest { Page = 1, PageSize = 10 };

        // Act
        var result = await _controller.GetCardType(request);

        // Assert
        result.Should().BeSameAs(expected);
        await _mockCardService.Received(1).GetCardType(request);
    }

    [Fact(DisplayName = "GetCardFlag calls ICardService and returns Ok")]
    public async Task GetCardFlag_ValidRequest_CallsServiceAndReturnsResult()
    {
        // Arrange
        var expected = new OkObjectResult(new ResponseApiSucess("ok"));
        _mockCardService.GetCardFlag(Arg.Any<GetCardFlagRequest>())
            .Returns(Task.FromResult<IActionResult>(expected));

        var request = new GetCardFlagRequest { Page = 1, PageSize = 10 };

        // Act
        var result = await _controller.GetCardFlag(request);

        // Assert
        result.Should().BeSameAs(expected);
        await _mockCardService.Received(1).GetCardFlag(request);
    }
}
