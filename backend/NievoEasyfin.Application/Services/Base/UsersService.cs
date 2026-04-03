using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Validator;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Services.Auth;
using NievoEasyfin.Application.Helper;
namespace NievoEasyfin.Application.Services.Base.Users
{
    public class UsersService : Controller
    {
        private static AuthService _AuthService;
        public UsersService(AuthService authService)
        {
            _AuthService = authService;
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

            string hash = await _AuthService.ConvertRequestPasswordToStringAsync(request.Password);

            var userEmail = await _AuthService.GetUserByEmailAsync(request.Email);
            if (userEmail != null)
                return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_ALREADY_EXISTS.GetDescription() }
                ));

            var user = await _AuthService.CreateUserAsync(request.Name, hash, request.Email);

            return StatusCode(201, new ResponseApiSucess(user));
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

            var provider = await _AuthService.GetProviderByNameAsync(request.Provider);
            if (provider == null)
                return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGURED.GetDescription() }
                ));

            var validateProvider = await _AuthService.ProviderValidateAsync(provider, request.ProviderAccessToken);
            if (validateProvider.Error != null)
                return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_200_RESPONSE.GetDescription() }
                ));


            // TODO: valida o token
            // TODO: Validar o usuario
            // TODO: Registrar o usuário na tabela de usuário
            // TODO: Registrar o usuário na tabela de user-provider

            return StatusCode(201, new ResponseApiSucess(request));
        }
    }
}