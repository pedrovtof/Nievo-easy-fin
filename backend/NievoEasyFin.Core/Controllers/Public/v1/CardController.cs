using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Services.Security;

namespace NievoEasyFin.Core.Controllers.Public;

/// <summary>
/// Controller responsible for managing card.
/// </summary>
[ApiController]
[Route("api/public/v1/[controller]")]
public class CardController : Controller
{
    private readonly ICardService _cardService;

    private readonly JsonWebTokenService _jsonWebTokenService;

    public CardController(ICardService cardService, JsonWebTokenService jsonWebTokenService)
    {
        _cardService = cardService;
        _jsonWebTokenService = jsonWebTokenService;
    }

    /// <summary>
    /// Get card types
    /// </summary>
    [HttpGet("type")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCardType([FromQuery] GetCardTypeRequest request)
        => await _cardService.GetCardType(request);

    /// <summary>
    /// Get card flags
    /// </summary>
    [HttpGet("flag")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCardFlag([FromQuery] GetCardFlagRequest request)
        => await _cardService.GetCardFlag(request);
}
