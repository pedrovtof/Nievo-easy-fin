using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Services.Security;

namespace NievoEasyFin.Core.Controllers.Public;

/// <summary>
/// Controller responsible for managing bank.
/// </summary>
[ApiController]
[Route("api/public/v1/[controller]")]
public class BankController : Controller
{
    private readonly IPublicBankService _bankService;

    private readonly JsonWebTokenService _jsonWebTokenService;

    public BankController(IPublicBankService bankService, JsonWebTokenService jsonWebTokenService)
    {
        _bankService = bankService;
        _jsonWebTokenService = jsonWebTokenService;
    }

    /// <summary>
    /// Get list of banks
    /// </summary>
    [HttpGet("")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBanks([FromQuery] GetBanksRequest request)
        => await _bankService.GetBanks(request);

    /// <summary>
    /// Get bank cards
    /// </summary>
    [HttpGet("card")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBankCard([FromHeader] string authorization, [FromQuery] GetBankCardRequest request)
    {
        request.SetEmail(
           await _jsonWebTokenService.GetClaimValue(authorization, "email")
       );
        return await _bankService.GetBankCard(request);
    }
}
