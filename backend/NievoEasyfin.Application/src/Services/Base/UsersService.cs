using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Validator;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Services.Auth;
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
        /// Method service for create user
        /// </summary>
        /// <param name="request">request PostCreateUserRequest</param>
        /// <returns>ResponseApiSucess/ResponseApiError</returns>
        public async Task<IActionResult> PostCreateUserAsync(PostCreateUserRequest request)
        {
            var validationResult = await new PostCreateUserValidator().ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                ResponseApiError error = new ResponseApiError(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
                return BadRequest(error);
            }

            string hash = await _AuthService.ConvertRequestPasswordToStringAsync(request.Password);

            var userEmail = await _AuthService.GetUserByEmailAsync(request.Email);
            if (userEmail != null)
            {
                // TODO: validar erro
                ResponseApiError error = new ResponseApiError(new List<string>() { EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_ALREADY_EXISTS.ToString() });
                return BadRequest(error);
            }

            var user = await _AuthService.CreateUserAsync(request.Name, hash, request.Email);

            return StatusCode(201, new ResponseApiSucess(user));
        }
    }
}