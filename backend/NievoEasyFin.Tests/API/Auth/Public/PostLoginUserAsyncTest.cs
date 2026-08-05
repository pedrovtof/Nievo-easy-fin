using NievoEasyFin.Tests.Mocks.Helpers;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using NievoEasyFin.Tests.Mocks.Infrastructure;
using Xunit;
using Xunit.Abstractions;
using NievoEasyFin.Tests.Build.Request;

namespace NievoEasyFin.Tests.API.Auth.Public;

public class PostLoginUserAsyncTest : AuthenticatorServiceTestBase
{
    public PostLoginUserAsyncTest(WireMockFixture fixture, ITestOutputHelper output) 
        : base(fixture, output)
    {
    }

    #region Success
    [Fact(DisplayName = "PostLoginUserAsync: With valid credentials, returns Ok with token")]
    public async Task PostLoginUserAsync_WithValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        Output.WriteLine("Arranging valid credentials login test.");
        var password = "Strong@123";
        var user = UserEntityFaker.Create().Generate();
        user.Password = await CryptoPasswordService.HashPasswordAsync(password);

        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();

        origin.Users.Add(user);
        await origin.SaveChangesAsync();

        var request = new PostLoginUserRequestBuilder();
        request.Email = user.Email!;
        request.Password = password;

        await DbContextMockFactory.SyncToAttachedDatabasesAsync(origin);

        var service = CreateService(origin, replica);

        // Act
        Output.WriteLine("Executing PostLoginUserAsync.");
        var result = await service.PostLoginUserAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();
    }
    #endregion
}
