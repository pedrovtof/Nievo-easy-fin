using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Configuration;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Models;
using NievoEasyFin.Application.Services.Base.Authenticator;
using NievoEasyFin.Application.Services.Security;
using NievoEasyFin.Application.Infrastructure.Auth;
using NievoEasyFin.Application.Services.Cache;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using NievoEasyFin.Tests.Mocks.Helpers;
using NievoEasyFin.Tests.Mocks.Infrastructure;
using WireMock.Server;
using Moq;
using StackExchange.Redis;
using Xunit;
using Xunit.Abstractions;
using System.IO;
using System;

namespace NievoEasyFin.Tests.API.Auth.Public;

[Collection("WireMock collection")]
public abstract class AuthenticatorServiceTestBase : IDisposable
{
    protected readonly CryptoPasswordService CryptoPasswordService;
    protected readonly JsonWebTokenService JsonWebTokenService;
    protected readonly WireMockServer WireMockServer;
    protected readonly ITestOutputHelper Output;

    static AuthenticatorServiceTestBase()
    {
        DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

        var googleId = "test-google-client-id"; // Value from .env
        Environment.SetEnvironmentVariable("JWT_PRIVATE_CONTRACT_STRING", "super-secret-private-key-long-enough-32-chars");
        Environment.SetEnvironmentVariable("JWT_PUBLIC_CONTRACT_STRING", "super-secret-public-key-long-enough-32-chars");
        Environment.SetEnvironmentVariable("GOOGLE_ID_CLIENT", googleId);
    }

    protected AuthenticatorServiceTestBase(WireMockFixture fixture, ITestOutputHelper output)
    {
        CryptoPasswordService = new CryptoPasswordService();
        JsonWebTokenService = new JsonWebTokenService(new JsonWebTokenConfiguration());
        WireMockServer = fixture.Server;
        WireMockServer.Reset();
        Output = output;
    }

    public void Dispose()
    {
    }

    protected AuthenticatorService CreateService(
        AuthOrigin origin,
        AuthReplica replica,
        AuthDbCacheService? cache = null,
        SmtpModel? smtp = null)
    {
        var userModel = new UserModel(origin, replica);
        var userProviderSsoModel = new UserProviderSsoModel(origin, replica);
        var ssoProviderAuth = new SSoProviderAuth(replica);
        var acceptTerms = new AcceptTermsModel(origin, replica);

        return new AuthenticatorService(
            CryptoPasswordService,
            cache ?? MockHelper.CreateMockedCacheService(new Mock<IDatabase>()),
            userModel,
            userProviderSsoModel,
            JsonWebTokenService,
            ssoProviderAuth,
            new SmtpProvider(),
            smtp ?? new SmtpModel(),
            acceptTerms
        );
    }
}
