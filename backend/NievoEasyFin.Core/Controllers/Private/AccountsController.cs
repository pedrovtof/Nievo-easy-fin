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
    /// Creates a new bank account for the authenticated user.
    /// </summary>
    /// <param name="authorization">The authorization token from the request header.</param>
    /// <param name="request">The bank account creation request data.</param>
    /// <response code = "200">Return created with sucess</response>
    /// <response code = "400">BadRequest</response>
    [HttpPost("banks")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAccountsBanks([FromHeader(Name = "Authorization")] string authorization, [FromBody] PostAccountsBanksRequest request)
        => await _accountsService.PostAccountsBanks(request);
}
