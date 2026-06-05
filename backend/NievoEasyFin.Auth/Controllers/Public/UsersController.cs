using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Interfaces.Response;
using Microsoft.AspNetCore.Authorization;

namespace NievoEasyFin.Auth.Controllers.Public;

[ApiController]
[Route("api/public/v1/[controller]")]
public class UsersController : Controller
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService users)
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
    ///        "email": "Joe.Black@example.com",
    ///        "accept_terms":true
    ///     }
    /// </remarks>
    /// <param name="UserAgent">User agent header</param> 
    /// <param name="Host">Host header</param> 
    /// <param name="request">Data from user (Name, Password, Email)</param>
    /// <response code="201">Create with sucess</response>
    /// <response code="400">Invalid request</response>
    /// <response code="409">User already exists</response>
    [HttpPost("singup")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostCreateUserAsync([FromHeader(Name = "User-Agent")] string UserAgent, [FromHeader(Name = "Host")] string Host, [FromBody] PostCreateUserRequest request)
    {
        request.SetUserAgent(UserAgent);
        request.SetHost(Host);

        return await _usersService.PostCreateUserAsync(request);
    }

    /// <summary>
    /// Endpoint to create user with SSO login
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST v1/Users/singup/sso
    ///     {
    ///        "provider_name": "google",
    ///        "provider_access_token": "yc29.aJKASDJLASD_jasdkasASJSAD-askldaj...",
    ///        "accept_terms":true
    ///     }
    /// </remarks>
    /// <param name="UserAgent">User agent header</param> 
    /// <param name="Host">Host header</param> 
    /// <param name="request">request.provider_name and request.provider_access_token</param>
    [HttpPost("singup-sso")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostCreateUserSsoAsync([FromHeader(Name = "User-Agent")] string UserAgent, [FromHeader(Name = "Host")] string Host, [FromBody] PostCreateUserSsoRequest request)
    {
        request.SetUserAgent(UserAgent);
        request.SetHost(Host);

        return await _usersService.PostCreateUserSsoAsync(request);
    }
}
