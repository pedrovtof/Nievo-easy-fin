using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Models;
using NievoEasyFin.Application.Services.Base.Users;
using NievoEasyFin.Application.Services.Security;
using NievoEasyFin.Application.Infrastructure.Auth;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using NievoEasyFin.Tests.Mocks.Helpers;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace NievoEasyFin.Tests.Application.Services.Base;

[Collection("WireMock collection")]
public class UsersServiceTests : IDisposable
{
    private readonly CryptoPasswordService _cryptoPasswordService;
    private readonly WireMockServer _wireMockServer;

    static UsersServiceTests()
    {
        // Load env for all tests in this class
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        DotNetEnv.Env.Load(envPath);

        var googleId = "test-google-client-id"; // Value from .env
        // Ensure variables are also in Environment
        Environment.SetEnvironmentVariable("REGEX_PASSWORD_RULE", "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,12}$");
        Environment.SetEnvironmentVariable("PASSWORD_CRYPTO_ITERATIONS", "350000");
        Environment.SetEnvironmentVariable("PASSWORD_CRYPTO_KEYSIZE", "64");
        Environment.SetEnvironmentVariable("PASSWORD_CRYPTO_SALT", "4142434445464748494A4B4C4D4E4F505152535455565758595A6162636465666768696A6B6C6D6E6F707172737475767778797A31323334353637383930");

        Environment.SetEnvironmentVariable("GOOGLE_ID_CLIENT", googleId);
    }

    public UsersServiceTests(WireMockFixture fixture)
    {
        _cryptoPasswordService = new CryptoPasswordService();
        _wireMockServer = fixture.Server;
        _wireMockServer.Reset();
    }

    public void Dispose()
    {
        // No need to stop server here as it's static, but could reset
    }

    [Fact]
    public async Task PostCreateUserAsync_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = PostCreateUserRequestFaker.Create().Generate();
        request.Password = "Strong@123";

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var userModel = new UserModel(authOrigin, authReplica);
        var userProviderSsoModel = new UserProviderSsoModel(authOrigin, authReplica);
        var ssoProviderAuth = new SSoProviderAuth(authReplica);

        var service = new UsersService(_cryptoPasswordService, userModel, userProviderSsoModel, ssoProviderAuth);

        // Act
        var result = await service.PostCreateUserAsync(request);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(201);
        objectResult.Value.Should().BeOfType<ResponseApiSucess>();

        // Verify database
        var userInDb = await authReplica.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        userInDb.Should().NotBeNull();
        userInDb!.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task PostCreateUserAsync_WhenEmailExists_ReturnsBadRequest()
    {
        // Arrange
        var request = PostCreateUserRequestFaker.Create().Generate();
        request.Password = "Strong@123";

        // Seed an existing user
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.Email;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var userModel = new UserModel(authOrigin, authReplica);
        var userProviderSsoModel = new UserProviderSsoModel(authOrigin, authReplica);
        var ssoProviderAuth = new SSoProviderAuth(authReplica);

        var service = new UsersService(_cryptoPasswordService, userModel, userProviderSsoModel, ssoProviderAuth);

        // Act
        var result = await service.PostCreateUserAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("já existe") || e.Contains("already exists"));
    }

    [Fact]
    public async Task PostCreateUserSsoAsync_WhenProviderNotFound_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostCreateUserSsoRequest { Provider = "unknown", ProviderAccessToken = "token" };

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var userModel = new UserModel(authOrigin, authReplica);
        var userProviderSsoModel = new UserProviderSsoModel(authOrigin, authReplica);
        var ssoProviderAuth = new SSoProviderAuth(authReplica);

        var service = new UsersService(_cryptoPasswordService, userModel, userProviderSsoModel, ssoProviderAuth);

        // Act
        var result = await service.PostCreateUserSsoAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("unknown") || e.Contains("não configurado"));
    }

    [Fact]
    public async Task PostCreateUserSsoAsync_WhenUserAlreadyExists_ReturnsOk()
    {
        // Arrange
        var request = new PostCreateUserSsoRequest { Provider = "google", ProviderAccessToken = "valid-token" };

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed provider
        var provider = new SsoProviderEntity { Id = 1, Name = "google", Active = true };
        authOrigin.SsoProvider.Add(provider);

        // Seed user and SSO link
        var user = UserEntityFaker.Create().Generate();
        authOrigin.Users.Add(user);
        await authOrigin.SaveChangesAsync();

        var subId = "google-sub-123";
        var userSso = new UserProviderSsoEntity { SsoProviderId = provider.Id, UserId = user.Id, Sub = subId };
        authOrigin.UserProvider.Add(userSso);
        await authOrigin.SaveChangesAsync();

        // Consistent with .env or what SSoProviderAuth might have read
        var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_ID_CLIENT") ?? "test-google-client-id";

        // Configure WireMock for Google
        _wireMockServer
            .Given(Request.Create().WithPath("/tokeninfo").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody($"{{\"aud\": \"{googleClientId}\"}}"));

        _wireMockServer
            .Given(Request.Create().WithPath("/userinfo").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody($"{{\"sub\": \"{subId}\", \"email\": \"{user.Email}\", \"name\": \"{user.Name}\"}}"));

        var userModel = new UserModel(authOrigin, authReplica);
        var userProviderSsoModel = new UserProviderSsoModel(authOrigin, authReplica);
        var ssoProviderAuth = new SSoProviderAuth(authReplica);

        var service = new UsersService(_cryptoPasswordService, userModel, userProviderSsoModel, ssoProviderAuth);

        // Act
        var result = await service.PostCreateUserSsoAsync(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();
    }

    [Fact]
    public async Task PostCreateUserSsoAsync_WhenNewUser_ReturnsCreated()
    {
        // Arrange
        var request = new PostCreateUserSsoRequest { Provider = "google", ProviderAccessToken = "new-token" };

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed provider
        var provider = new SsoProviderEntity { Id = 1, Name = "google", Active = true };
        authOrigin.SsoProvider.Add(provider);
        await authOrigin.SaveChangesAsync();

        var subId = "google-sub-new";
        var userEmail = "new-sso-user@example.com";
        var userName = "New SSO User";

        // Consistent with .env or what SSoProviderAuth might have read
        var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_ID_CLIENT") ?? "test-google-client-id";

        // Configure WireMock for Google
        _wireMockServer
            .Given(Request.Create().WithPath("/tokeninfo").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody($"{{\"aud\": \"{googleClientId}\"}}"));

        _wireMockServer
            .Given(Request.Create().WithPath("/userinfo").UsingGet())
            .RespondWith(Response.Create().WithStatusCode(200).WithBody($"{{\"sub\": \"{subId}\", \"email\": \"{userEmail}\", \"name\": \"{userName}\"}}"));

        var userModel = new UserModel(authOrigin, authReplica);
        var userProviderSsoModel = new UserProviderSsoModel(authOrigin, authReplica);
        var ssoProviderAuth = new SSoProviderAuth(authReplica);

        var service = new UsersService(_cryptoPasswordService, userModel, userProviderSsoModel, ssoProviderAuth);

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

        // Verify database
        var userInDb = await authReplica.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        userInDb.Should().NotBeNull();
        userInDb!.Name.Should().Be(userName);

        var ssoLink = await authReplica.UserProvider.FirstOrDefaultAsync(ups => ups.Sub == subId && ups.UserId == userInDb.Id);
        ssoLink.Should().NotBeNull();
    }
}
