using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NievoEasyFin.Tests.Mocks.Database;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

public class UserServiceDirectTest : AccountsServiceTestBase
{
    public UserServiceDirectTest(ITestOutputHelper output) : base(output) { }

    [Fact(DisplayName = "GetUserBanks directly on UserService with invalid page returns BadRequest")]
    public async Task GetUserBanks_DirectUserService_InvalidPage_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetUserBanksRequestBuilder().WithEmail("");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateUserService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetUserBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().NotBeEmpty();
    }
}
