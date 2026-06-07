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
using NievoEasyFin.Application.Services.Cache;

namespace NievoEasyFin.Application.Services.Base.Users;

/// <summary>
/// Service responsible for user management, including standard and SSO registration.
/// </summary>
public class UsersService : Controller, IUsersService
{
    private readonly CryptoPasswordService _cryptoPasswordService;

    private readonly UserModel _userModel;

    private readonly SSoProviderAuth _ssoProviderAuth;

    private readonly UserProviderSsoModel _userProviderSsoModel;

    private readonly SmtpModel _smtpModel;

    private readonly AuthDbCacheService _authDbCacheService;

    private readonly AcceptTermsModel _acceptTermsmodel;

    private readonly UsersAcceptedTermsModel _usersAcceptedTermsModel;

    public UsersService(
        CryptoPasswordService cryptoPasswordService,
        UserModel userModel,
        UserProviderSsoModel userProviderSsoModel,
        SSoProviderAuth ssoProviderAuth,
        SmtpModel smtpModel,
        AuthDbCacheService authDbCacheService,
        AcceptTermsModel acceptTermsmodel,
        UsersAcceptedTermsModel usersAcceptedTermsModel
    )
    {
        _cryptoPasswordService = cryptoPasswordService;
        _userModel = userModel;
        _userProviderSsoModel = userProviderSsoModel;
        _ssoProviderAuth = ssoProviderAuth;
        _smtpModel = smtpModel;
        _authDbCacheService = authDbCacheService;
        _acceptTermsmodel = acceptTermsmodel;
        _usersAcceptedTermsModel = usersAcceptedTermsModel;
    }

    /// <summary>
    /// Creates a new user account with email and password.
    /// </summary>
    /// <param name="request">The user creation request data.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the user creation.</returns>
    public async Task<IActionResult> PostCreateUserAsync(PostCreateUserRequest request)
    {
        var validationResult = await new PostCreateUserValidator().ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(
                new ResponseApiError(validationResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var userEmail = await _userModel.GetUserByEmailWithAnyStatusAsync(request.Email);
        if (userEmail != null)
        {
            if (userEmail.StatusId == (int)EnumUserStatus.INVALID)
                return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_NOT_VALIDATED.GetDescription() }
                ));

            return BadRequest(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_ALREADY_EXISTS.GetDescription() }
            ));
        }

        var tk = await _authDbCacheService.CreateTokenSingupUser(request.Email, request.Name);

        var smtp = await _smtpModel.SingUpUserTokenMailAsync(request.Email, tk.PinToken);

        string hash = await _cryptoPasswordService.HashPasswordAsync(request.Password);

        var user = await _userModel.CreateUserAsync(request.Name, hash, request.Email, (int)EnumUserStatus.INVALID);

        bool createdAcceptTerms = await CreateAcceptTermsUsers(request.GetHost(), request.GetUserAgent(), request.AcceptTerms, user.Id);
        if (!createdAcceptTerms)
            return BadRequest(new ResponseApiError(
            new List<string>() { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_ERROR_WHILE_ACCEPT_TERMS.GetDescription() }
        ));

        return StatusCode(201, new ResponseApiSucess(EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_201_CREATED.GetDescription()));
    }

    /// <summary>
    /// Creates a new user account or links an existing one via an SSO provider.
    /// </summary>
    /// <param name="request">The SSO user creation request data.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the SSO user creation.</returns>
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
                user = await _userModel.CreateUserAsync($"{validateProvider.Name}", null, validateProvider.Email, (int)EnumUserStatus.ACTIVE);

            var userProviderSso = await _userProviderSsoModel.CreateUserProviderSsoEntityAsync(provider.Id, user.Id, validateProvider.Sub);

            bool createdAcceptTerms = await CreateAcceptTermsUsers(request.GetHost(), request.GetUserAgent(), request.AcceptTerms, user.Id);
            if (!createdAcceptTerms)
                return BadRequest(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_ERROR_WHILE_ACCEPT_TERMS.GetDescription() }
            ));

            return StatusCode(201, new ResponseApiSucess(EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_201_CREATED.GetDescription()));
        }
        else
            return Ok(new ResponseApiSucess(EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_200_USER_ALREADY_EXISTS.GetDescription()));
    }

    /// <summary>
    /// Method to create accept terms users
    /// </summary>
    /// <returns>true or false</returns>
    public async Task<bool> CreateAcceptTermsUsers(string host, string userAgent, bool acceptedTerms, int userId)
    {
        if (!acceptedTerms)
            return false;

        var acceptTermsEntity = await _acceptTermsmodel.GetAcceptTermsWithCodeAsync(_acceptTermsmodel.GetCodeSingupTerms());
        if (acceptTermsEntity == null)
            return false;

        var usersAcceptedTerms = await _usersAcceptedTermsModel.CreateUsersAcceptedTermsEntityAsync(acceptTermsEntity.Id, host, userAgent, acceptedTerms, userId);
        if (usersAcceptedTerms == null)
            return false;

        return true;
    }
}
