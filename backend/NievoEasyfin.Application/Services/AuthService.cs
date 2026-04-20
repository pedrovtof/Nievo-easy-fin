using NievoEasyfin.Application.Models;
using NievoEasyfin.Application.Data.Entities;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Infrastructure.Auth;
using NievoEasyfin.Application.Services.Security;
namespace NievoEasyfin.Application.Services.Auth;

/// <summary>
/// General class to support services
/// </summary>
public class AuthService
{
    private static CryptoPasswordService _cryptoPasswordService;

    private static UserModel _userModel;

    private static SSoProviderAuth _ssoProviderAuth;

    private static UserProviderSsoModel _userProviderSsoModel;

    private static JsonWebTokenService _jsonWebTokenService;

    public AuthService(
        CryptoPasswordService cryptoPasswordService,
        UserModel userModel,
        SSoProviderAuth ssoProviderAuth,
        UserProviderSsoModel userProviderSsoModel,
        JsonWebTokenService jsonWebTokenService
    )
    {
        _cryptoPasswordService = cryptoPasswordService;
        _userModel = userModel;
        _ssoProviderAuth = ssoProviderAuth;
        _userProviderSsoModel = userProviderSsoModel;
        _jsonWebTokenService = jsonWebTokenService;
    }

    #region Crypto

    /// <summary>
    /// Method to convert password into hashPass
    /// </summary>
    /// <param name="password">request password</param>
    /// <returns>Hash password</returns>
    public async Task<string> ConvertRequestPasswordToStringAsync(string password)
        => await _cryptoPasswordService.HashPasswordAsync(password);

    /// <summary>
    /// Method to validate if the password is correct
    /// </summary>
    /// <param name="password">password from request</param>
    /// <param name="hash">hash from database</param>
    /// <returns>true/false</returns>
    public async Task<bool> ValidateHashPasswordAsync(string password, string hash)
        => await _cryptoPasswordService.HashValidateAsync(password, hash);

    #endregion Crypto

    #region User

    /// <summary>
    /// Create user entity
    /// </summary>
    /// <param name="name">request.name</param>
    /// <param name="password">request.password</param>
    /// <param name="email">request.email</param>
    /// <returns>userView</returns>
    public async Task<UserEntity> CreateUserAsync(string name, string password, string email)
        => await _userModel.CreateUserAsync(name, password, email);

    /// <summary>
    /// Create user entity
    /// </summary>
    /// <param name="name">provider.response.name</param>
    /// <param name="email">provider.response.password</param>
    /// <param name="sub">provider.response.email</param>
    /// <returns>userView</returns>
    public async Task<UserEntity> CreateUserSsoAsync(string name, string email, string sub)
        => await _userModel.CreateUserSsoAsync(name, email, sub);

    public async Task<UserProviderSsoEntity> CreateUserProviderSsoEntityAsync(int provider, int user, string sub)
    {
        return await _userProviderSsoModel.CreateUserProviderSsoEntityAsync(provider, user, sub);
    }

    /// <summary>
    /// Search user by email
    /// </summary>
    /// <param name="email">email</param>
    /// <returns>UserEntity</returns>
    public async Task<UserEntity> GetUserByEmailAsync(string email)
        => await _userModel.GetUserByEmailAsync(email);

    /// <summary>
    /// Search user by email and status
    /// </summary>
    /// <param name="email">email</param>
    /// <returns>UserEntity</returns>
    public async Task<UserEntity> GetUserByEmailWithAnyStatusAsync(string email)
        => await _userModel.GetUserByEmailWithAnyStatusAsync(email);

    #endregion User

    #region Provider

    /// <summary>
    /// Search user-provider by sub and provider
    /// </summary>
    /// <param name="sub">Unique id</param>
    /// <param name="provider">provider Id</param>
    /// <returns></returns>
    public async Task<UserProviderSsoEntity> GetUserProviderSsoBySubAndProviderAsync(string sub, int provider)
        => await _userProviderSsoModel.GetUserProviderSsoBySubAndProviderAsync(sub, provider);

    /// <summary>
    ///  Search provider by name
    /// </summary>
    /// <param name="provider">name of the provider</param>
    /// <returns>SsoProviderEntity</returns>
    public async Task<SsoProviderEntity> GetProviderByNameAsync(string provider)
        => await _ssoProviderAuth.GetProviderByNameAsync(provider);

    /// <summary>
    /// Search User by providerSub and providerId
    /// </summary>
    /// <param name="subId">string SubId</param>
    /// <param name="providerId">int ProviderId</param>
    /// <returns>UserEntity</returns>
    public async Task<UserEntity> GetUserByProviderSubAndIdAsync(string subId, int providerId)
        => await _userModel.GetUserByProviderSubAndIdAsync(subId, providerId);

    /// <summary>
    /// Method to validate the type of provider from sso
    /// </summary>
    /// <param name="provider">SsoProviderEntity</param>
    /// <param name="token">response string token from api sso</param>
    /// <returns></returns>
    public async Task<ResponseProvider> ProviderValidateAsync(string provider, string token)
        => await _ssoProviderAuth.ValidateProviderAsync(provider, token);

    #endregion Provider

    #region Token

    /// <summary>
    /// Method to generate token JWT
    /// </summary>
    /// <param name="email">User email from database</param>
    /// <returns>token JWT</returns>
    public async Task<string> GenerateTokenJwtAsync(string email)
        => await _jsonWebTokenService.GenerateTokenAsync(email);

    #endregion Token
}
