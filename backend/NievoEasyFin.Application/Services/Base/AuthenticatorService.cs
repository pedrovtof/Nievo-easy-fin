using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Infrastructure.Auth;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Validator;
using NievoEasyFin.Application.Models;
using NievoEasyFin.Application.Services.Cache;
using NievoEasyFin.Application.Services.Security;

using NievoEasyFin.Application.Interfaces.Services;

namespace NievoEasyFin.Application.Services.Base.Authenticator;

/// <summary>
/// Implementation of the <see cref="IAuthenticatorService"/>, handling the core logic for user authentication, SSO, and password management.
/// </summary>
public class AuthenticatorService : Controller, IAuthenticatorService
{
    private readonly CryptoPasswordService _cryptoPasswordService;

    private readonly UserModel _userModel;

    private readonly AuthDbCacheService _authDbCacheService;

    private readonly UserProviderSsoModel _userProviderSsoModel;

    private readonly JsonWebTokenService _jsonWebTokenService;

    private readonly SSoProviderAuth _ssoProviderAuth;

    private readonly SmtpModel _smtpModel;

    public AuthenticatorService(
        CryptoPasswordService cryptoPasswordService,
        AuthDbCacheService authDbCacheService,
        UserModel userModel,
        UserProviderSsoModel userProviderSsoModel,
        JsonWebTokenService jsonWebTokenService,
        SSoProviderAuth ssoProviderAuth,
        SmtpProvider smtpProvider,
        SmtpModel smtpModel
    )
    {
        _cryptoPasswordService = cryptoPasswordService;
        _authDbCacheService = authDbCacheService;
        _userModel = userModel;
        _userProviderSsoModel = userProviderSsoModel;
        _jsonWebTokenService = jsonWebTokenService;
        _ssoProviderAuth = ssoProviderAuth;
        _smtpModel = smtpModel;
    }

    /// <summary>
    /// Processes a standard login request using email and password.
    /// </summary>
    /// <param name="request">The login request data.</param>
    /// <returns>An <see cref="IActionResult"/> with the JWT token on success, or error details on failure.</returns>
    public async Task<IActionResult> PostLoginUserAsync(PostLoginUserRequest request)
    {
        var validatorResult = await new PostLoginUserValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var user = await _userModel.GetUserByEmailAsync(request.Email);
        if (user == null)
            return NotFound(new ResponseApiError(
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
    /// Processes an SSO login request using a third-party provider.
    /// </summary>
    /// <param name="request">The SSO login request data.</param>
    /// <returns>An <see cref="IActionResult"/> with the JWT token on success, or error details on failure.</returns>
    /// Method service to Login Sso 
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
    /// Initiates the password reset process by generating a token and sending it via email.
    /// </summary>
    /// <param name="request">The request containing the user's email.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the reset initiation.</returns>
    public async Task<IActionResult> PostResetPasswordAsync(PostResetPasswordRequest request)
    {
        var validationResult = await new PostResetPasswordValidator().ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(
                new ResponseApiError(validationResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var user = await _userModel.GetUserByEmailAsync(request.Email);
        if (user == null)
            return NotFound(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUNND.GetDescription() }
            ));

        var tokenResetPassword = await _authDbCacheService.GetTokenPasswordResetAttempByUserIdAsync(user.Id);
        if (tokenResetPassword == null)
        {
            var tk = await _authDbCacheService.CreateTokenPasswordResetAttempAsync(user.Id, user.Email);

            var smtp = await _smtpModel.ResetTokenMailAsync(user.Email, tk.PinToken);

            return StatusCode(
                201,
                new ResponseApiSucess(EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_201_USER_TOKEN.GetDescription()
            ));
        }
        else
            return NotFound(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_USER_TOKEN_FOUND_IN_CACHE.GetDescription() }
            ));
    }

    /// <summary>
    /// Completes the password reset process by validating the token and updating the password in the database.
    /// </summary>
    /// <param name="request">The request containing the email, token, and new password.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the password change.</returns>
    public async Task<IActionResult> PatchResetPasswordAsync(PatchResetPasswordRequest request)
    {
        var validationResult = await new PatchResetPasswordValidator().ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(
                new ResponseApiError(validationResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var user = await _userModel.GetUserByEmailAsync(request.Email);
        if (user == null)
            return NotFound(new ResponseApiError(
                    new List<string>() { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUND.GetDescription() }
            ));

        var tokenResetPassword = await _authDbCacheService.GetTokenPasswordResetAttempByUserIdAsync(user.Id);
        if (tokenResetPassword == null)
            return NotFound(new ResponseApiError(
                    new List<string>() { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_404_USER_TOKEN_NOT_FOUND_IN_CACHE.GetDescription() }
            ));

        var validateToken = await _authDbCacheService.ValidateTokenAsync(request.PinToken, tokenResetPassword.PinToken);
        if (!validateToken)
            return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_TOKEN_INVALID.GetDescription() }
            ));

        var hashPassword = await _cryptoPasswordService.HashPasswordAsync(request.Password);
        if (hashPassword == user.Password)
            return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_IS_THE_SAME.GetDescription() }
            ));

        var resetPassword = await _userModel.UpdateUserPasswordAsync(user.Id, hashPassword);
        if (!resetPassword)
            return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_NOT_UPDATED.GetDescription() }
            ));

        return Ok(new ResponseApiSucess(
            EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_200_PASSWORD_CHANGED.GetDescription()
        ));
    }

    /// <summary>
    /// Completes the validate email process by validating the token and updating the user status in the database.
    /// </summary>
    /// <param name="request">The request containing the email and token.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the process.</returns>
    public async Task<IActionResult> PostValidateEmailAsync(PostValidateEmailRequest request)
    {
        var validationResult = await new PostValidateEmailValidator().ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(
                new ResponseApiError(validationResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var user = await _userModel.GetUserAllByEmailAsync(request.Email);
        if (user == null)
            return NotFound(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_404_USER_NOT_FOUND.GetDescription() }
            ));

        if (user.StatusId == (int)EnumUserStatus.ACTIVE || user.StatusId == (int)EnumUserStatus.INACTIVE)
            return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_404_USER_BLOCKED_OR_VALIDATED.GetDescription() }
            ));

        var tokenResetPassword = await _authDbCacheService.GetTokenEmailValidateAsync(user.Email);
        if (tokenResetPassword == null)
            return NotFound(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_404_TOKEN_NOTFOUND_IN_CACHE.GetDescription() }
            ));

        var validateToken = await _authDbCacheService.ValidateTokenAsync(request.PinToken, tokenResetPassword.PinToken);
        if (!validateToken)
            return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_404_WRONG_TOKEN.GetDescription() }
            ));

        var updateStatus = await _userModel.UpdateUserStatusAsync(user.Id, (int)EnumUserStatus.ACTIVE);
        if (updateStatus)
            return Ok(new ResponseApiSucess(
                EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_200_USER_VALIDATED.GetDescription()
            ));
        else
            return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_200_ERROR_VALIDATE_EMAIL.GetDescription() }
            ));
    }



    /// <summary>
    /// Stub — implementation in progress.
    /// </summary>
    public Task<IActionResult> PostValidateEmailSendAsync(PostValidateEmailSendRequest request)
        => throw new NotImplementedException();

}
