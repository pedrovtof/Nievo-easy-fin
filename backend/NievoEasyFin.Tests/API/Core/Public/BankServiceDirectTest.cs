using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NievoEasyFin.Tests.Mocks.Database;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

public class BankServiceDirectTest : AccountsServiceTestBase
{
    public BankServiceDirectTest(ITestOutputHelper output) : base(output) { }

    [Fact(DisplayName = "GetBanks directly on BankService with invalid page returns BadRequest")]
    public async Task GetBanks_DirectBankService_InvalidPage_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetBanksRequestBuilder();
        request.Page = 0;
        var (origin, replica) = CreateSharedCoreContexts();
        var service = CreateBankService(origin, replica);

        // Act
        var result = await service.GetBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().NotBeEmpty();
    }
}
