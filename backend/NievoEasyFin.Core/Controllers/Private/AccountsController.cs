using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Services.Security;

namespace NievoEasyFin.Core.Controllers.Private;

/// <summary>
/// Controller responsible for managing user accounts and bank accounts operations.
/// </summary>
[ApiController]
[Route("api/private/v1/[controller]")]
public class AccountsController : Controller
{
    private readonly IAccountsService _accountsService;

    private readonly JsonWebTokenService _jsonWebTokenService;

    public AccountsController(IAccountsService accountsService, JsonWebTokenService jsonWebTokenService)
    {
        _accountsService = accountsService;
        _jsonWebTokenService = jsonWebTokenService;
    }

    /// <summary>
    /// Creates a new bank account.
    /// </summary>
    /// <param name="request">The bank account creation request data.</param>
    /// <response code = "200">Return created with sucess</response>
    /// <response code = "400">BadRequest</response>
    [HttpPost("banks")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAccountsBanks([FromBody] PostAccountsBanksRequest request)
        => await _accountsService.PostAccountsBanks(request);

    /// <summary>
    /// Get list of banks
    /// </summary>
    [HttpGet("banks")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBanks([FromQuery] GetBanksRequest request)
        => await _accountsService.GetBanks(request);

    /// <summary>
    /// Creates a new user bank account.
    /// </summary>
    /// <param name="authorization">Token JWT</param>
    /// <param name="request">PostUserBanksRequest</param>
    /// <returns>IActionResult</returns>
    [HttpPost("user-banks")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostUserBanks([FromHeader] string authorization, [FromBody] PostUserBanksRequest request)
    {
        request.SetEmail(
            await _jsonWebTokenService.GetClaimValue(authorization, "email")
        );
        return await _accountsService.PostUserBanks(request);
    }

    /// <summary>
    /// Get user bank accounts.
    /// </summary>
    /// <param name="authorization">Token JWT</param>
    /// <param name="request">GetUserBanksRequest</param>
    /// <returns>IActionResult</returns>
    [HttpGet("user-banks")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserBanks([FromHeader] string authorization, [FromBody] GetUserBanksRequest request)
    {
        request.SetEmail(
            await _jsonWebTokenService.GetClaimValue(authorization, "email")
        );
        return await _accountsService.GetUserBanks(request);
    }
}
