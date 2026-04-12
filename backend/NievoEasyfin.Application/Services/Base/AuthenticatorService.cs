using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Data.Context.Database;
using NievoEasyfin.Application.Extensions.Enum;
using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Interfaces.Validator;
using NievoEasyfin.Application.Services.Auth;

namespace NievoEasyfin.Application.Services.Base.Authenticator
{
    public class AuthenticatorService : Controller
    {
        private static AuthService _authService;

        public AuthenticatorService(AuthService authService)
        {
            _authService = authService;
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

            var user = await _authService.GetUserByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_NOT_FOUND.GetDescription() }
                ));

            var passwordValid = await _authService.ValidateHashPasswordAsync(request.Password, user.Password);
            if (!passwordValid)
                return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_WRONG_PASSWORD.GetDescription() }
                ));

            var generateToken = await _authService.GenerateTokenJwtAsync(user.Email);

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

            var provider = await _authService.GetProviderByNameAsync(request.Provider);
            if (provider == null)
                return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGURED.GetDescription() }
                ));

            var validateProvider = await _authService.ProviderValidateAsync(provider.Name, request.ProviderAccessToken);
            if (validateProvider.Error != null)
                return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_200_RESPONSE.GetDescription() }
                ));

            var userSub = await _authService.GetUserProviderSsoBySubAndProviderAsync(validateProvider.Sub, provider.Id);
            if (userSub == null)
                return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTLOGINUSERSSOASYNC_AUTHSERVICE_400_PROVIDERSSO_NOT_CONFIGURED.GetDescription() }
                ));

            else
            {
                var user = await _authService.GetUserByProviderSubAndIdAsync(validateProvider.Sub, provider.Id);
                var generateToken = await _authService.GenerateTokenJwtAsync(user.Email);
                return Ok(new ResponseApiSucess(
                    new { Token = generateToken }
                ));
            }
        }
    }
}
