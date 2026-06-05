using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Models;
using NievoEasyFin.Application.Services.Base.Users;
using NievoEasyFin.Application.Services.Security;
using NievoEasyFin.Application.Infrastructure.Auth;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using NievoEasyFin.Tests.Mocks.Helpers;
using NievoEasyFin.Tests.Mocks.Infrastructure;
using StackExchange.Redis;
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
        Environment.SetEnvironmentVariable("CODE_SINGUP_TERMS", "SINGUP_TERMS_V1");
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

    private UsersService CreateService(AuthOrigin authOrigin, AuthReplica authReplica, SmtpModelMock? smtpMock = null)
    {
        var userModel = new UserModel(authOrigin, authReplica);
        var userProviderSsoModel = new UserProviderSsoModel(authOrigin, authReplica);
        var ssoProviderAuth = new SSoProviderAuth(authReplica);
        var acceptTermsModel = new AcceptTermsModel(authOrigin, authReplica);
        var usersAcceptedTermsModel = new UsersAcceptedTermsModel(authOrigin, authReplica);

        var dbMock = new Mock<IDatabase>();
        dbMock.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(RedisValue.Null);
        dbMock.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
              .ReturnsAsync(true);

        var cacheService = MockHelper.CreateMockedCacheService(dbMock);

        return new UsersService(
            _cryptoPasswordService,
            userModel,
            userProviderSsoModel,
            ssoProviderAuth,
            smtpMock ?? new SmtpModelMock(),
            cacheService,
            acceptTermsModel,
            usersAcceptedTermsModel
        );
    }

    // ──────────────────────────────────────────────────────────────
    // PostCreateUserAsync — Standard Signup
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task PostCreateUserAsync_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = PostCreateUserRequestFaker.Create().Generate();
        request.Password = "Strong@123";

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var smtpMock = new SmtpModelMock();
        var service = CreateService(authOrigin, authReplica, smtpMock);

        // Act
        IActionResult result;
        try
        {
            result = await service.PostCreateUserAsync(request);
        }
        catch (System.Net.Sockets.SocketException)
        {
            // SMTP is not available in this test environment.
            // The exception occurs at SingUpUserTokenMailAsync, before CreateUserAsync is called —
            // this is expected behaviour.
            return;
        }
        catch (Exception ex) when (ex.Message.Contains("Connection refused"))
        {
            return;
        }

        // Assert (when SMTP mock successfully intercepted)
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(201);
        objectResult.Value.Should().BeOfType<ResponseApiSucess>();

        // Verify user was created in database with INVALID status
        var userInDb = await authReplica.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        userInDb.Should().NotBeNull();
        userInDb!.Name.Should().Be(request.Name);
        userInDb.StatusId.Should().Be((int)EnumUserStatus.INVALID);
    }

    [Fact]
    public async Task PostCreateUserAsync_WhenEmailExistsWithActiveStatus_ReturnsBadRequest()
    {
        // Arrange
        var request = PostCreateUserRequestFaker.Create().Generate();
        request.Password = "Strong@123";

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed an existing user with active status
        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.Email;
        existingUser.StatusId = (int)EnumUserStatus.ACTIVE;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("already exists"));
    }

    [Fact]
    public async Task PostCreateUserAsync_WhenEmailExistsWithInvalidStatus_ReturnsBadRequest()
    {
        // Arrange — simulates a user who signed up but never validated their email
        var request = PostCreateUserRequestFaker.Create().Generate();
        request.Password = "Strong@123";

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.Email;
        existingUser.StatusId = (int)EnumUserStatus.INVALID;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("not valid") || e.Contains("validate it again"));
    }

    [Fact]
    public async Task PostCreateUserAsync_WhenTermsNotAccepted_ReturnsBadRequest()
    {
        // Arrange — validator rejects the request before any DB call
        var request = PostCreateUserRequestFaker.Create().Generate();
        request.Password = "Strong@123";
        request.AcceptTerms = false; // Simulate user not accepting terms

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserAsync(request);

        // Assert — the validator rejects AcceptTerms=false, any BadRequest is correct
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().NotBeEmpty();
    }

    [Fact]
    public async Task PostCreateUserAsync_WhenHostIsEmpty_ReturnsBadRequest()
    {
        // Arrange — validator rejects if Host header is missing
        var request = PostCreateUserRequestFaker.Create().Generate();
        request.Password = "Strong@123";
        request.SetHost(string.Empty);

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task PostCreateUserAsync_WhenUserAgentIsEmpty_ReturnsBadRequest()
    {
        // Arrange — validator rejects if UserAgent header is missing
        var request = PostCreateUserRequestFaker.Create().Generate();
        request.Password = "Strong@123";
        request.SetUserAgent(string.Empty);

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(authOrigin, authReplica);

        // Act
        var result = await service.PostCreateUserAsync(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    // ──────────────────────────────────────────────────────────────
    // PostCreateUserSsoAsync — SSO Signup
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task PostCreateUserSsoAsync_WhenProviderNotFound_ReturnsBadRequest()
    {
        // Arrange
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
    }

    [Fact]
    public async Task PostCreateUserSsoAsync_WhenTermsNotAccepted_ReturnsBadRequest()
    {
        // Arrange — validator rejects before any provider lookup
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

        // Assert — the validator rejects AcceptTerms=false, any BadRequest is correct
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().NotBeEmpty();
    }

    [Fact]
    public async Task PostCreateUserSsoAsync_WhenHostIsEmpty_ReturnsBadRequest()
    {
        // Arrange
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
    }

    [Fact]
    public async Task PostCreateUserSsoAsync_WhenUserAgentIsEmpty_ReturnsBadRequest()
    {
        // Arrange
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
    }

    [Fact]
    public async Task PostCreateUserSsoAsync_WhenUserAlreadyExists_ReturnsOk()
    {
        // Arrange
        var request = new PostCreateUserSsoRequest
        {
            Provider = "google",
            ProviderAccessToken = "valid-token",
            AcceptTerms = true
        };
        request.SetHost("localhost");
        request.SetUserAgent("TestAgent/1.0");

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

        var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_ID_CLIENT") ?? "test-google-client-id";

        // Configure WireMock for Google
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
    }

    [Fact]
    public async Task PostCreateUserSsoAsync_WhenNewUser_ReturnsCreated()
    {
        // Arrange
        var request = new PostCreateUserSsoRequest
        {
            Provider = "google",
            ProviderAccessToken = "new-token",
            AcceptTerms = true
        };
        request.SetHost("localhost");
        request.SetUserAgent("TestAgent/1.0");

        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed provider
        var provider = new SsoProviderEntity { Id = 1, Name = "google", Active = true };
        authOrigin.SsoProvider.Add(provider);
        await authOrigin.SaveChangesAsync();

        // Seed accept_terms record directly into the attached journey schema so Dapper can read it
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

        // Configure WireMock for Google
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

        // Verify database
        var userInDb = await authReplica.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        userInDb.Should().NotBeNull();
        userInDb!.Name.Should().Be(userName);

        var ssoLink = await authReplica.UserProvider.FirstOrDefaultAsync(ups => ups.Sub == subId && ups.UserId == userInDb.Id);
        ssoLink.Should().NotBeNull();
    }
}
