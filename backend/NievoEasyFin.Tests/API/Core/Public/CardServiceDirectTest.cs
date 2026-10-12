using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

public class CardServiceDirectTest : AccountsServiceTestBase
{
    public CardServiceDirectTest(ITestOutputHelper output) : base(output) { }

    [Fact(DisplayName = "GetCardType directly on CardService with invalid page returns BadRequest")]
    public async Task GetCardType_DirectCardService_InvalidPage_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetCardTypeRequestBuilder();
        request.Page = 0;
        var (origin, replica) = CreateSharedCoreContexts();
        var service = CreateCardService(origin, replica);

        // Act
        var result = await service.GetCardType(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().NotBeEmpty();
    }
}
