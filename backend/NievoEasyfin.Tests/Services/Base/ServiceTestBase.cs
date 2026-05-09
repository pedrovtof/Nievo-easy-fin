using NievoEasyfin.Application.Interfaces.Infrastructure;
using NievoEasyfin.Application.Interfaces.Models;
using NievoEasyfin.Application.Interfaces.Services;
using NievoEasyfin.Application.Services.Base.Authenticator;
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyfin.Tests.Services.Base;

public abstract class ServiceTestBase
{
    protected readonly ITestOutputHelper Output;
    
    // Mocks para AuthenticatorService
    protected readonly ICryptoPasswordService MockCrypto;
    protected readonly IAuthDbCacheService MockCache;
    protected readonly IUserModel MockUserModel;
    protected readonly IUserProviderSsoModel MockUserSsoModel;
    protected readonly IJsonWebTokenService MockJwt;
    protected readonly ISSoProviderAuth MockSsoAuth;
    protected readonly ISmtpModel MockSmtp;

    protected readonly AuthenticatorService AuthenticatorService;

    protected ServiceTestBase(ITestOutputHelper output)
    {
        Output = output;

        MockCrypto = Substitute.For<ICryptoPasswordService>();
        MockCache = Substitute.For<IAuthDbCacheService>();
        MockUserModel = Substitute.For<IUserModel>();
        MockUserSsoModel = Substitute.For<IUserProviderSsoModel>();
        MockJwt = Substitute.For<IJsonWebTokenService>();
        MockSsoAuth = Substitute.For<ISSoProviderAuth>();
        MockSmtp = Substitute.For<ISmtpModel>();

        AuthenticatorService = new AuthenticatorService(
            MockCrypto,
            MockCache,
            MockUserModel,
            MockUserSsoModel,
            MockJwt,
            MockSsoAuth,
            MockSmtp
        );
    }
}
