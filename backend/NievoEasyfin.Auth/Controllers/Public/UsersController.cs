using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Services.Base.Users;
using NievoEasyfin.Application.Interfaces.Response;
using Microsoft.AspNetCore.Authorization;

namespace NievoEasyfin.Auth.Controllers.Public;

[ApiController]
[Route("api/public/v1/[controller]")]
public class UsersController : Controller
{
    private readonly UsersService _usersService;

    public UsersController(UsersService users)
    {
        _usersService = users;
    }

    /// <summary>
    /// Endpoints to create user
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST v1/Users/singup
    ///     {
    ///        "name": "Joe Black",
    ///        "password": "1Meet-Death",
    ///        "email": "Joe.Black@example.com"
    ///     }
    /// </remarks>
    /// <param name="request">Data from user (Name, Password, Email)</param>
    /// <response code="201">Create with sucess</response>
    /// <response code="400">Invalid request</response>
    /// <response code="409">User already exists</response>
    [HttpPost("singup")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostCreateUserAsync([FromBody] PostCreateUserRequest request)
        => await _usersService.PostCreateUserAsync(request);

    /// <summary>
    /// Endpoint to create user with SSO login
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST v1/Users/singup/sso
    ///     {
    ///        "provider_name": "google",
    ///        "provider_access_token": "yc29.aJKASDJLASD_jasdkasASJSAD-askldaj..."
    ///     }
    /// </remarks>
    /// <param name="request">request.provider_name and request.provider_access_token</param>
    [HttpPost("singup-sso")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostCreateUserSsoAsync([FromBody] PostCreateUserSsoRequest request)
        => await _usersService.PostCreateUserSsoAsync(request);
}
