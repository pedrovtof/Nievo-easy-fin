using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Validator;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Services.Security;
using NievoEasyFin.Application.Models;
using NievoEasyFin.Application.Infrastructure.Auth;
using NievoEasyFin.Application.Interfaces.Services;

namespace NievoEasyFin.Application.Services.Base.Users;

public class UsersService : Controller, IUsersService
{
    private readonly CryptoPasswordService _cryptoPasswordService;

    private readonly UserModel _userModel;

    private readonly SSoProviderAuth _ssoProviderAuth;

    private readonly UserProviderSsoModel _userProviderSsoModel;


    public UsersService(
        CryptoPasswordService cryptoPasswordService,
        UserModel userModel,
        UserProviderSsoModel userProviderSsoModel,
        SSoProviderAuth ssoProviderAuth
    )
    {
        _cryptoPasswordService = cryptoPasswordService;
        _userModel = userModel;
        _userProviderSsoModel = userProviderSsoModel;
        _ssoProviderAuth = ssoProviderAuth;
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

        string hash = await _cryptoPasswordService.HashPasswordAsync(request.Password);

        var userEmail = await _userModel.GetUserByEmailWithAnyStatusAsync(request.Email);
        if (userEmail != null)
            return BadRequest(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_ALREADY_EXISTS.GetDescription() }
            ));

        var user = await _userModel.CreateUserAsync(request.Name, hash, request.Email);

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

        var provider = await _ssoProviderAuth.GetProviderByNameAsync(request.Provider);
        if (provider == null)
            return BadRequest(new ResponseApiError(
                new List<string>() {
                    EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGURED.GetDescription(),
                    EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_INACTIVE.GetDescription()
                }
            ));

        var validateProvider = await _ssoProviderAuth.ValidateProviderAsync(provider.Name, request.ProviderAccessToken);
        if (validateProvider.Error != null)
            return BadRequest(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_200_RESPONSE.GetDescription() }
            ));

        var userSub = await _userProviderSsoModel.GetUserProviderSsoBySubAndProviderAsync(validateProvider.Sub, provider.Id);
        if (userSub == null)
        {
            var user = await _userModel.GetUserByEmailWithAnyStatusAsync(validateProvider.Email);
            if (user == null)
                user = await _userModel.CreateUserAsync($"{validateProvider.Name}", null, validateProvider.Email);

            var userProviderSso = await _userProviderSsoModel.CreateUserProviderSsoEntityAsync(provider.Id, user.Id, validateProvider.Sub);
            return StatusCode(201, new ResponseApiSucess(EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_201_CREATED.GetDescription()));
        }
        else
            return Ok(new ResponseApiSucess(EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_200_USER_ALREADY_EXISTS.GetDescription()));
    }
}
