using NievoEasyFin.Tests.Mocks.Helpers;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using NievoEasyFin.Tests.Mocks.Infrastructure;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;
using Xunit.Abstractions;
using NievoEasyFin.Tests.Build.Request;

namespace NievoEasyFin.Tests.API.Auth.Public;

public class PostLoginUserSsoAsyncTest : AuthenticatorServiceTestBase
{
    public PostLoginUserSsoAsyncTest(WireMockFixture fixture, ITestOutputHelper output) 
        : base(fixture, output)
    {
    }

    #region Success
    [Fact(DisplayName = "PostLoginUserSsoAsync: When user exists, returns Ok with token")]
    public async Task PostLoginUserSsoAsync_WhenUserExists_ReturnsOkWithToken()
    {
        // Arrange
        Output.WriteLine("Arranging user SSO test.");
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();

        var provider = new SsoProviderEntity { Id = 1, Name = "google", Active = true };
        origin.SsoProvider.Add(provider);
        await origin.SaveChangesAsync();

        var user = UserEntityFaker.Create().Generate();
        origin.Users.Add(user);
        await origin.SaveChangesAsync();

        var subId = "google-sub-123";
        var userSso = new UserProviderSsoEntity { SsoProviderId = provider.Id, UserId = user.Id, Sub = subId };
        origin.UserProvider.Add(userSso);
        await origin.SaveChangesAsync();

        await DbContextMockFactory.SyncToAttachedDatabasesAsync(origin);

        var googleClientId = "test-google-client-id";

        WireMockServer
            .Given(Request.Create().WithPath("/tokeninfo").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody($"{{\"aud\": \"{googleClientId}\"}}"));

        WireMockServer
            .Given(Request.Create().WithPath("/userinfo").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody($"{{\"sub\": \"{subId}\", \"email\": \"{user.Email}\", \"name\": \"{user.Name}\"}}"));

        var request = new PostLoginUserSsoRequestBuilder();
        request.Provider = "google";

        var service = CreateService(origin, replica);

        // Act
        Output.WriteLine("Executing PostLoginUserSsoAsync.");
        var result = await service.PostLoginUserSsoAsync(request);

        // Assert
        Output.WriteLine("Validating result.");
        if (result is BadRequestObjectResult badRequest)
        {
            var error = (ResponseApiError)badRequest.Value!;
            throw new Exception($"Login SSO failed with: {string.Join(", ", error.Messages)}");
        }
        result.Should().BeOfType<OkObjectResult>();
    }
    #endregion
}
