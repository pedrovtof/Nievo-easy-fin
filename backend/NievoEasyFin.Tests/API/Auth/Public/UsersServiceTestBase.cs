using System;
using System.IO;
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
using Xunit;
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
