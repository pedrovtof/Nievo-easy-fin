using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Services.Base.Authenticator;
using NievoEasyfin.Application.Interfaces.Response;
using Microsoft.AspNetCore.Authorization;

namespace NievoEasyfin.Auth.Controllers.Public
{
    [ApiController]
    [Route("api/public/v1/[controller]")]
    public class AuthenticatorController : Controller
    {
        private static AuthenticatorService _authenticatorService;
        public AuthenticatorController(AuthenticatorService authenticatorService)
        {
            _authenticatorService = authenticatorService;
        }

        /// <summary>
        /// Endpoint for normal SingIn (login)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST v1/Users/singin
        ///     {
        ///        "email": "Joe.Black@example.com",
        ///        "password": "1Meet-Death"
        ///     }
        /// </remarks>
        /// <param name="request">Data from user (Email, Password)</param>
        [HttpPost("singin")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostLoginUserAsync([FromBody] PostLoginUserRequest request)
            => await _authenticatorService.PostLoginUserAsync(request);


        /// <summary>
        /// Endpoint for sso Singin (login)
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST v1/Users/singin-sso
        ///     {
        ///        "provider_name": "google",
        ///        "provider_access_token": "yc29.aJKASDJLASD_jasdkasASJSAD-askldaj..."
        ///     }
        /// </remarks>
        /// <param name="request">request.provider_name and request.provider_access_token</param>
        [HttpPost("singin-sso")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostLoginUserSsoAsync([FromBody] PostLogiPostLoginUserSsoRequest request)
            => await _authenticatorService.PostLoginUserSsoAsync(request);
    }
}
