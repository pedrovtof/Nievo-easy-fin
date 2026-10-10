using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Services.Security;

namespace NievoEasyFin.Core.Controllers.Public;

/// <summary>
/// Controller responsible for managing user.
/// </summary>
[ApiController]
[Route("api/public/v1/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    private readonly JsonWebTokenService _jsonWebTokenService;

    public UserController(IUserService userService, JsonWebTokenService jsonWebTokenService)
    {
        _userService = userService;
        _jsonWebTokenService = jsonWebTokenService;
    }

    /// <summary>
    /// Creates a new user bank account.
    /// </summary>
    /// <param name="authorization">Token JWT</param>
    /// <param name="request">PostUserBanksRequest</param>
    /// <returns>IActionResult</returns>
    [HttpPost("banks")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostUserBanks([FromHeader] string authorization, [FromBody] PostUserBanksRequest request)
    {
        request.SetEmail(
            await _jsonWebTokenService.GetClaimValue(authorization, "email")
        );
        return await _userService.PostUserBanks(request);
    }

    /// <summary>
    /// Get user bank accounts.
    /// </summary>
    /// <param name="authorization">Token JWT</param>
    /// <param name="request">GetUserBanksRequest</param>
    /// <returns>IActionResult</returns>
    [HttpGet("banks")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserBanks([FromHeader] string authorization, [FromQuery] GetUserBanksRequest request)
    {
        request.SetEmail(
            await _jsonWebTokenService.GetClaimValue(authorization, "email")
        );
        return await _userService.GetUserBanks(request);
    }

    /// <summary>
    /// Get user card banks
    /// </summary>
    [HttpGet("bank-card")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserCard([FromHeader] string authorization, [FromQuery] GetUserCardRequest request)
    {
        request.SetEmail(
           await _jsonWebTokenService.GetClaimValue(authorization, "email")
       );
        return await _userService.GetUserCard(request);
    }

    /// <summary>
    /// Create user card banks
    /// </summary>
    [HttpPost("bank-card")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostUserCard([FromHeader] string authorization, [FromBody] PostUserCardRequest request)
    {
        request.SetEmail(
           await _jsonWebTokenService.GetClaimValue(authorization, "email")
       );
        return await _userService.PostUserCard(request);
    }

    /// <summary>
    /// Create an user goals
    /// </summary>
    [HttpPost("goal")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostUserGoal([FromHeader] string authorization, [FromBody] PostUserGoalRequest request)
    {
        request.SetEmail(
           await _jsonWebTokenService.GetClaimValue(authorization, "email")
       );
        return await _userService.PostUserGoal(request);
    }

    /// <summary>
    /// Get user goals
    /// </summary>
    [HttpGet("goal")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserGoal([FromHeader] string authorization, [FromQuery] GetUserGoalRequest request)
    {
        request.SetEmail(
           await _jsonWebTokenService.GetClaimValue(authorization, "email")
       );
        return await _userService.GetUserGoal(request);
    }

    /// <summary>
    /// Create an user category
    /// </summary>
    [HttpPost("category")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostUserCategory([FromHeader] string authorization, [FromBody] PostUserCategoryRequest request)
    {
        request.SetEmail(
           await _jsonWebTokenService.GetClaimValue(authorization, "email")
       );
        return await _userService.PostUserCategory(request);
    }

    /// <summary>
    /// Get user category
    /// </summary>
    [HttpGet("category")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserCategory([FromHeader] string authorization, [FromQuery] GetUserCategoryRequest request)
    {
        request.SetEmail(
           await _jsonWebTokenService.GetClaimValue(authorization, "email")
       );
        return await _userService.GetUserCategory(request);
    }
}
