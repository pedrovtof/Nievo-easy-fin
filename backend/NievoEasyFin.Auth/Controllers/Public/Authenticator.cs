using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using Microsoft.AspNetCore.Authorization;
using NievoEasyFin.Application.Interfaces.Services;

namespace NievoEasyFin.Auth.Controllers.Public;

/// <summary>
/// Controller responsible for public authentication endpoints, including login, SSO, and password recovery.
/// </summary>
[ApiController]
[Route("api/public/v1/[controller]")]
public class AuthenticatorController : Controller
{
    private readonly IAuthenticatorService _authenticatorService;

    public AuthenticatorController(IAuthenticatorService authenticatorService)
    {
        _authenticatorService = authenticatorService;
    }

    /// <summary>
    /// Endpoint for normal SingIn (login)
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST v1/Authenticator/singin
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
    ///     POST v1/Authenticator/singin-sso
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

    /// <summary>
    /// Endpoint to create token for reset password
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST v1/Authenticator/password-reset
    ///     {
    ///        "email": "Joe.Black@example.com"
    ///     }
    /// </remarks>
    /// <param name="request">request.email</param>
    [HttpPost("password-reset")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostResetPasswordAsync([FromBody] PostResetPasswordRequest request)
        => await _authenticatorService.PostResetPasswordAsync(request);

    /// <summary>
    /// Endpoint to reset password with token
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH v1/Authenticator/password-reset
    ///     {
    ///        "email": "Joe.Black@example.com",
    ///        "pin_token": "111111",
    ///        "password": "1Meet-Death"
    ///     }
    /// </remarks>
    /// <param name="request">request.pin_token and request.email</param>
    [HttpPatch("password-reset")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PatchResetPasswordAsync([FromBody] PatchResetPasswordRequest request)
        => await _authenticatorService.PatchResetPasswordAsync(request);
}
