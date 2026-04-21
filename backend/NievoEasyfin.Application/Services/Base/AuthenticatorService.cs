using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Extensions.Enum;
using NievoEasyfin.Application.Infrastructure.Auth;
using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Interfaces.Validator;
using NievoEasyfin.Application.Models;
using NievoEasyfin.Application.Services.Cache;
using NievoEasyfin.Application.Services.Security;

namespace NievoEasyfin.Application.Services.Base.Authenticator;

public class AuthenticatorService : Controller
{
    private readonly CryptoPasswordService _cryptoPasswordService;

    private readonly UserModel _userModel;

    private readonly AuthDbCacheService _authDbCacheService;

    private readonly UserProviderSsoModel _userProviderSsoModel;

    private readonly JsonWebTokenService _jsonWebTokenService;

    private readonly SSoProviderAuth _ssoProviderAuth;


    public AuthenticatorService(
        CryptoPasswordService cryptoPasswordService,
        AuthDbCacheService authDbCacheService,
        UserModel userModel,
        UserProviderSsoModel userProviderSsoModel,
        JsonWebTokenService jsonWebTokenService,
        SSoProviderAuth ssoProviderAuth
    )
    {
        _cryptoPasswordService = cryptoPasswordService;
        _authDbCacheService = authDbCacheService;
        _userModel = userModel;
        _userProviderSsoModel = userProviderSsoModel;
        _jsonWebTokenService = jsonWebTokenService;
        _ssoProviderAuth = ssoProviderAuth;
    }

    /// <summary>
    /// Method service to login
    /// </summary>
    /// <param name="request">request PostLoginUserRequest</param>
    public async Task<IActionResult> PostLoginUserAsync(PostLoginUserRequest request)
    {
        var validatorResult = await new PostLoginUserValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var user = await _userModel.GetUserByEmailAsync(request.Email);
        if (user == null)
            return BadRequest(new ResponseApiError(
                new List<string>() {
                    EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_NOT_FOUND.GetDescription(),
                    EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_BLOCKED.GetDescription()
                    }
            ));

        var passwordValid = await _cryptoPasswordService.HashValidateAsync(request.Password, user.Password);
        if (!passwordValid)
            return BadRequest(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_WRONG_PASSWORD.GetDescription() }
            ));

        var generateToken = await _jsonWebTokenService.GenerateTokenAsync(user.Email);

        return Ok(new ResponseApiSucess(
            new { Token = generateToken }
        ));
    }

    /// <summary>
    /// Method service to Login Sso 
    /// </summary>
    /// <param name="request">request PostLoginUserRequest</param>
    public async Task<IActionResult> PostLoginUserSsoAsync(PostLogiPostLoginUserSsoRequest request)
    {
        var validatorResult = await new PostLoginUserSsoValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var provider = await _ssoProviderAuth.GetProviderByNameAsync(request.Provider);
        if (provider == null)
            return BadRequest(new ResponseApiError(
                new List<string>() {
                    EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGURED.GetDescription(),
                    EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_INACTIVE.GetDescription()
                }
            ));

        var validateProvider = await _ssoProviderAuth.ValidateProviderAsync(provider.Name, request.ProviderAccessToken);
        if (validateProvider.Error != null)
            return BadRequest(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_200_RESPONSE.GetDescription() }
            ));

        var userSub = await _userProviderSsoModel.GetUserProviderSsoBySubAndProviderAsync(validateProvider.Sub, provider.Id);
        if (userSub == null)
            return BadRequest(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDERSSO_NOT_CONFIGURED.GetDescription() }
            ));

        else
        {
            var user = await _userModel.GetUserByProviderSubAndIdAsync(validateProvider.Sub, provider.Id);
            if (user == null)
                return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_USER_BLOCKED.GetDescription() }
                ));

            var generateToken = await _jsonWebTokenService.GenerateTokenAsync(user.Email);
            return Ok(new ResponseApiSucess(
                new { Token = generateToken }
            ));
        }
    }

    /// <summary>
    /// Method service for reset password
    /// </summary>
    /// <param name="request">request.email</param>
    public async Task<IActionResult> PostResetPasswordAsync(PostResetPasswordRequest request)
    {
        var validationResult = await new PostResetPasswordValidator().ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(
                new ResponseApiError(validationResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var user = await _userModel.GetUserByEmailAsync(request.Email);
        if (user == null)
            return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_USER_NOT_FOUNND.GetDescription() }
            ));

        // TODO: Criar model para SMTP 
        // TODO: Adicionar SMTP na chamada para enviar email
        // TODO: Adicionar teste no healtCheck

        var tokenResetPassword = await _authDbCacheService.GetTokenPasswordResetAttempByUserIdAsync(user.Id);
        if (tokenResetPassword == null)
        {
            await _authDbCacheService.CreateTokenPasswordResetAttempAsync(user.Id, user.Email);
            return StatusCode(
                201,
                new ResponseApiSucess(EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_201_USER_TOKEN.GetDescription()
            ));
        }
        else
            return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_USER_TOKEN_FOUND_IN_CACHE.GetDescription() }
            ));
    }

    /// <summary>
    /// Method service to change password in database
    /// </summary>
    /// <param name="request">requset.pin_token and request.email</param>
    /// <returns></returns>
    public async Task<IActionResult> PatchResetPasswordAsync(PatchResetPasswordRequest request)
    {
        var validationResult = await new PatchResetPasswordValidator().ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(
                new ResponseApiError(validationResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        return Ok();
    }
}
