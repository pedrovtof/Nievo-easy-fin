using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Services.Security;

namespace NievoEasyFin.Core.Controllers.Public.v1
;

/// <summary>
/// Controller responsible for payment.
/// </summary>
[ApiController]
[Route("api/public/v1/[controller]")]
public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;

    private readonly JsonWebTokenService _jsonWebTokenService;

    public PaymentController(IPaymentService paymentService, JsonWebTokenService jsonWebTokenService)
    {
        _paymentService = paymentService;
        _jsonWebTokenService = jsonWebTokenService;
    }

    /// <summary>
    /// Creates a new user payment transaction.
    /// </summary>
    /// <param name="authorization">Token JWT</param>
    /// <param name="request">PostPaymentTransactionRequest</param>
    /// <returns>IActionResult</returns>
    [HttpPost("transaction")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostPaymentTransaction([FromHeader] string authorization, [FromBody] PostPaymentTransactionRequest request)
    {
        request.SetEmail(
            await _jsonWebTokenService.GetClaimValue(authorization, "email")
        );
        return await _paymentService.PostPaymentTransaction(request);
    }
}
