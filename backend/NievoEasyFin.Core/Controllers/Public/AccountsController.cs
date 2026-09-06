using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Services.Security;

namespace NievoEasyFin.Core.Controllers.Public;

/// <summary>
/// Controller responsible for managing user accounts and bank accounts operations.
/// </summary>
[ApiController]
[Route("api/public/v1/[controller]")]
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
    /// Get list of banks
    /// </summary>
    [HttpGet("banks")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBanks([FromQuery] GetBanksRequest request)
        => await _accountsService.GetBanks(request);

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
    public async Task<IActionResult> GetUserBanks([FromHeader] string authorization, [FromQuery] GetUserBanksRequest request)
    {
        request.SetEmail(
            await _jsonWebTokenService.GetClaimValue(authorization, "email")
        );
        return await _accountsService.GetUserBanks(request);
    }

    /// <summary>
    /// Get card types
    /// </summary>
    [HttpGet("card-type")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCardType([FromQuery] GetCardTypeRequest request)
        => await _accountsService.GetCardType(request);

    /// <summary>
    /// Get card flags
    /// </summary>
    [HttpGet("card-flag")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCardFlag([FromQuery] GetCardFlagRequest request)
        => await _accountsService.GetCardFlag(request);

    /// <summary>
    /// Get bank cards
    /// </summary>
    [HttpGet("bank-card")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBankCard([FromHeader] string authorization, [FromQuery] GetBankCardRequest request)
    {
        request.SetEmail(
           await _jsonWebTokenService.GetClaimValue(authorization, "email")
       );
        return await _accountsService.GetBankCard(request);
    }

    /// <summary>
    /// Get user card banks
    /// </summary>
    [HttpGet("user:bank-card")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserCard([FromHeader] string authorization, [FromQuery] GetUserCardRequest request)
    {
        request.SetEmail(
           await _jsonWebTokenService.GetClaimValue(authorization, "email")
       );
        return await _accountsService.GetUserCard(request);
    }

    /// <summary>
    /// Create user card banks
    /// </summary>
    [HttpPost("user:bank-card")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostUserCard([FromHeader] string authorization, [FromBody] PostUserCardRequest request)
    {
        request.SetEmail(
           await _jsonWebTokenService.GetClaimValue(authorization, "email")
       );
        return await _accountsService.PostUserCard(request);
    }
}
