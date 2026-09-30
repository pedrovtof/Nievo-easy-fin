using Moq;
using NievoEasyFin.Application.Models;
using NievoEasyFin.Application.Services.Base.Users;
using NievoEasyFin.Application.Services.Security;
using NievoEasyFin.Application.Infrastructure.Auth;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Tests.Mocks.Helpers;
using NievoEasyFin.Tests.Mocks.Infrastructure;
using StackExchange.Redis;
using WireMock.Server;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Auth.Public;

[Collection("WireMock collection")]
public abstract class UsersServiceTestBase : IDisposable
{
    protected readonly CryptoPasswordService _cryptoPasswordService;
    protected readonly WireMockServer _wireMockServer;
    protected readonly ITestOutputHelper Output;

    static UsersServiceTestBase()
    {
        TestEnvironment.Setup();
    }

    protected UsersServiceTestBase(WireMockFixture fixture, ITestOutputHelper output)
    {
        _cryptoPasswordService = new CryptoPasswordService();
        _wireMockServer = fixture.Server;
        _wireMockServer.Reset();
        Output = output;
    }

    public void Dispose()
    {
        // No need to stop server here as it's static, but could reset
    }

    protected UsersService CreateService(AuthOrigin authOrigin, AuthReplica authReplica, SmtpModelMock? smtpMock = null)
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
}
