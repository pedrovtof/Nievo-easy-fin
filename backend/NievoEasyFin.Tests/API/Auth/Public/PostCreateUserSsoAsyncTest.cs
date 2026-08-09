using NievoEasyFin.Tests.Mocks.Helpers;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using NievoEasyFin.Tests.Mocks.Infrastructure;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Auth.Public;

public class PostCreateUserSsoAsyncTest : UsersServiceTestBase
{
    public PostCreateUserSsoAsyncTest(WireMockFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    #region Success

    [Fact(DisplayName = "PostCreateUserSsoAsync: When user already exists returns Ok")]
    public async Task PostCreateUserSsoAsync_WhenUserAlreadyExists_ReturnsOk()
    {
        // Arrange
        Output.WriteLine("Setting up SSO signup for existing user");
        var request = new PostCreateUserSsoRequest
        {
            Provider = "google",
            ProviderAccessToken = "valid-token",
            AcceptTerms = true
        };
        request.SetHost("localhost");
        request.SetUserAgent("TestAgent/1.0");

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var provider = SsoProviderEntityFaker.Create().Generate();
        provider.Id = 1;
        provider.Name = "google";
        provider.Active = true;
        authOrigin.SsoProvider.Add(provider);

        var user = UserEntityFaker.Create().Generate();
        authOrigin.Users.Add(user);
        await authOrigin.SaveChangesAsync();

        var subId = "google-sub-123";
        var userSso = UserProviderSsoEntityFaker.Create().Generate();
        userSso.SsoProviderId = provider.Id;
        userSso.UserId = user.Id;
        userSso.Sub = subId;
        authOrigin.UserProvider.Add(userSso);
        await authOrigin.SaveChangesAsync();

        var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_ID_CLIENT") ?? "test-google-client-id";

        _wireMockServer
            .Given(Request.Create().WithPath("/tokeninfo").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody($"{{\"aud\": \"{googleClientId}\"}}"));

        _wireMockServer
            .Given(Request.Create().WithPath("/userinfo").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody($"{{\"sub\": \"{subId}\", \"email\": \"{user.Email}\", \"name\": \"{user.Name}\"}}"));

        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserSsoAsync(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();
        
        Output.WriteLine("Validation passed: returned Ok for existing SSO user");
    }

    [Fact(DisplayName = "PostCreateUserSsoAsync: When new user returns Created")]
    public async Task PostCreateUserSsoAsync_WhenNewUser_ReturnsCreated()
    {
        // Arrange
        Output.WriteLine("Setting up SSO signup for new user");
        var request = new PostCreateUserSsoRequest
        {
            Provider = "google",
            ProviderAccessToken = "new-token",
            AcceptTerms = true
        };
        request.SetHost("localhost");
        request.SetUserAgent("TestAgent/1.0");

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var provider = SsoProviderEntityFaker.Create().Generate();
        provider.Id = 1;
        provider.Name = "google";
        provider.Active = true;
        authOrigin.SsoProvider.Add(provider);
        await authOrigin.SaveChangesAsync();

        var code = Environment.GetEnvironmentVariable("CODE_SINGUP_TERMS") ?? "SINGUP_TERMS_V1";
        var connection = authOrigin.Database.GetDbConnection();
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = $"INSERT INTO journey.accept_terms (code, name, version, active, created_at, updated_at) VALUES ('{code}', 'Terms of Service', 1, 1, datetime('now'), datetime('now'));";
            cmd.ExecuteNonQuery();
        }

        var subId = "google-sub-new";
        var userEmail = "new-sso-user@example.com";
        var userName = "New SSO User";

        var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_ID_CLIENT") ?? "test-google-client-id";

        _wireMockServer
            .Given(Request.Create().WithPath("/tokeninfo").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody($"{{\"aud\": \"{googleClientId}\"}}"));

        _wireMockServer
            .Given(Request.Create().WithPath("/userinfo").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody($"{{\"sub\": \"{subId}\", \"email\": \"{userEmail}\", \"name\": \"{userName}\"}}"));

        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserSsoAsync(request);

        // Assert
        if (result is BadRequestObjectResult badRequest)
        {
            var error = (ResponseApiError)badRequest.Value!;
            throw new Exception($"Create User SSO failed with: {string.Join(", ", error.Messages)}");
        }
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(201);
        objectResult.Value.Should().BeOfType<ResponseApiSucess>();

        var userInDb = await authReplica.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        userInDb.Should().NotBeNull();
        userInDb!.Name.Should().Be(userName);

        var ssoLink = await authReplica.UserProvider.FirstOrDefaultAsync(ups => ups.Sub == subId && ups.UserId == userInDb.Id);
        ssoLink.Should().NotBeNull();
        
        Output.WriteLine("Validation passed: returned Created for new SSO user");
    }

    #endregion

    #region BadRequest Errors

    [Fact(DisplayName = "PostCreateUserSsoAsync: When provider not found returns BadRequest")]
    public async Task PostCreateUserSsoAsync_WhenProviderNotFound_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Setting up SSO signup with unknown provider");
        var request = new PostCreateUserSsoRequest
        {
            Provider = "unknown",
            ProviderAccessToken = "token",
            AcceptTerms = true
        };
        request.SetHost("localhost");
        request.SetUserAgent("TestAgent/1.0");

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserSsoAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("unknown") || e.Contains("not configured") || e.Contains("não configurado"));
        
        Output.WriteLine("Validation passed: returned BadRequest for unknown provider");
    }

    [Fact(DisplayName = "PostCreateUserSsoAsync: When terms not accepted returns BadRequest")]
    public async Task PostCreateUserSsoAsync_WhenTermsNotAccepted_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Setting up SSO signup with unaccepted terms");
        var request = new PostCreateUserSsoRequest
        {
            Provider = "google",
            ProviderAccessToken = "valid-token",
            AcceptTerms = false
        };
        request.SetHost("localhost");
        request.SetUserAgent("TestAgent/1.0");

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserSsoAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().NotBeEmpty();
        
        Output.WriteLine("Validation passed: returned BadRequest when terms not accepted");
    }

    [Fact(DisplayName = "PostCreateUserSsoAsync: When host is empty returns BadRequest")]
    public async Task PostCreateUserSsoAsync_WhenHostIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Setting up SSO signup with empty host");
        var request = new PostCreateUserSsoRequest
        {
            Provider = "google",
            ProviderAccessToken = "valid-token",
            AcceptTerms = true
        };
        request.SetHost(string.Empty);
        request.SetUserAgent("TestAgent/1.0");

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserSsoAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        Output.WriteLine("Validation passed: returned BadRequest for empty host");
    }

    [Fact(DisplayName = "PostCreateUserSsoAsync: When UserAgent is empty returns BadRequest")]
    public async Task PostCreateUserSsoAsync_WhenUserAgentIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Setting up SSO signup with empty UserAgent");
        var request = new PostCreateUserSsoRequest
        {
            Provider = "google",
            ProviderAccessToken = "valid-token",
            AcceptTerms = true
        };
        request.SetHost("localhost");
        request.SetUserAgent(string.Empty);

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserSsoAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        Output.WriteLine("Validation passed: returned BadRequest for empty UserAgent");
    }

    #endregion
}
