using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Configuration;
using NievoEasyFin.Application.Models;
using NievoEasyFin.Application.Services.Base.Authenticator;
using NievoEasyFin.Application.Services.Security;
using NievoEasyFin.Application.Infrastructure.Auth;
using NievoEasyFin.Application.Services.Cache;
using NievoEasyFin.Tests.Mocks.Helpers;
using WireMock.Server;
using Moq;
using StackExchange.Redis;
using Xunit.Abstractions;

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
        TestEnvironment.Setup();
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
