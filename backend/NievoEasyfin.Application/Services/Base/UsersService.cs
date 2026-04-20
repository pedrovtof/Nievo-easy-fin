using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Validator;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Extensions.Enum;
using NievoEasyfin.Application.Services.Auth;

using NievoEasyfin.Application.Data.Context.Cache;

namespace NievoEasyfin.Application.Services.Base.Users;

public class UsersService : Controller
{
    private readonly AuthService _authService;

    public UsersService(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Method service for create user basic
    /// </summary>
    /// <param name="request">request PostCreateUserRequest</param>
    /// <returns>ResponseApiSucess/ResponseApiError</returns>
    public async Task<IActionResult> PostCreateUserAsync(PostCreateUserRequest request)
    {
        var validationResult = await new PostCreateUserValidator().ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(
                new ResponseApiError(validationResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        string hash = await _authService.ConvertRequestPasswordToStringAsync(request.Password);

        var userEmail = await _authService.GetUserByEmailWithAnyStatusAsync(request.Email);
        if (userEmail != null)
            return BadRequest(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_ALREADY_EXISTS.GetDescription() }
            ));

        var user = await _authService.CreateUserAsync(request.Name, hash, request.Email);

        return StatusCode(201, new ResponseApiSucess(EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_201_CREATED.GetDescription()));
    }

    /// <summary>
    /// Method service for create user sso
    /// </summary>
    /// <param name="request">request PostCreateUserSsoAsync</param>
    /// <returns>ResponseApiSucess/ResponseApiError</returns>
    public async Task<IActionResult> PostCreateUserSsoAsync(PostCreateUserSsoRequest request)
    {
        var validatorResult = await new PostCreateUserSsoValidator().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var provider = await _authService.GetProviderByNameAsync(request.Provider);
        if (provider == null)
            return BadRequest(new ResponseApiError(
                new List<string>() {
                    EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGURED.GetDescription(),
                    EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_INACTIVE.GetDescription()
                }
            ));

        var validateProvider = await _authService.ProviderValidateAsync(provider.Name, request.ProviderAccessToken);
        if (validateProvider.Error != null)
            return BadRequest(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_200_RESPONSE.GetDescription() }
            ));

        var userSub = await _authService.GetUserProviderSsoBySubAndProviderAsync(validateProvider.Sub, provider.Id);
        if (userSub == null)
        {
            var user = await _authService.GetUserByEmailWithAnyStatusAsync(validateProvider.Email);
            if (user == null)
                user = await _authService.CreateUserAsync($"{validateProvider.Name}", null, validateProvider.Email);

            var userProviderSso = await _authService.CreateUserProviderSsoEntityAsync(provider.Id, user.Id, validateProvider.Sub);
            return StatusCode(201, new ResponseApiSucess(EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_201_CREATED.GetDescription()));
        }
        else
            return Ok(new ResponseApiSucess(EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_200_USER_ALREADY_EXISTS.GetDescription()));
    }

    /// <summary>
    /// Method service for reset password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<IActionResult> PatchResetPasswordAsync(PatchResetPasswordRequest request)
    {
        var validationResult = await new PatchResetPasswordValidator().ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(
                new ResponseApiError(validationResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var user = await _authService.GetUserByEmailAsync(request.Email);
        if (user == null)
            return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_USER_NOT_FOUNND.GetDescription() }
            ));

        // TODO: Criar model para SMTP 
        // TODO: Adicionar SMTP na chamada para enviar email
        // TODO: Adicionar teste no healtCheck

        var tokenResetPassword = await _authService.GetTokenPasswordResetAttempAsync(user.Id);
        if (tokenResetPassword == null)
        {
            await _authService.CreateTokenPasswordResetAttempAsync(user.Id, user.Email);
            return Ok();
        }
        else
            return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_USER_TOKEN_FOUND_IN_CACHE.GetDescription() }
            ));
    }
}
