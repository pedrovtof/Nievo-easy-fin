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
    /// Get user bank accounts.
    /// </summary>
    /// <param name="authorization">Token JWT</param>
    /// <returns>IActionResult</returns>
    [HttpGet("user-banks")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserBanks([FromHeader] string authorization)
    {
        GetUserBanksRequest request = new();
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
    /// Get bank cards
    /// </summary>
    [HttpGet("bank-cards")]
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
}
