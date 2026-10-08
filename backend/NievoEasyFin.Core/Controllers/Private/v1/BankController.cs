using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Services.Security;

namespace NievoEasyFin.Core.Controllers.Private;

/// <summary>
/// Controller responsible for managing bank operations.
/// </summary>
[ApiController]
[Route("api/private/v1/[controller]")]
public class BankController : Controller
{
    private readonly IAccountsService _accountsService;

    private readonly JsonWebTokenService _jsonWebTokenService;

    public BankController(IAccountsService accountsService, JsonWebTokenService jsonWebTokenService)
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
    [HttpPost("")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAccountsBanks([FromBody] PostAccountsBanksRequest request)
        => await _accountsService.PostAccountsBanks(request);

    /// <summary>
    /// Create an bank card
    /// </summary>
    [HttpPost("card")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostBankCard([FromBody] PostBankCardRequest request)
        => await _accountsService.PostBankCard(request);
}
