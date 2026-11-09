using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Services.Security;

namespace NievoEasyFin.Core.Controllers.Public;

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
    /// Search for transactions status
    /// </summary>
    /// <param name="authorization">Token JWT</param>
    /// <param name="request">GetPaymentTransactionStatusRequest</param>
    /// <returns></returns>
    [HttpGet("status")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPaymentTransactionStatus([FromHeader] string authorization, [FromQuery] GetPaymentTransactionStatusRequest request)
        => await _paymentService.GetPaymentTransactionStatus(request);

    /// <summary>
    /// Search for transactions type
    /// </summary>
    /// <param name="authorization">Token JWT</param>
    /// <param name="request">GetPaymentTransactionTypeRequest</param>
    /// <returns></returns>
    [HttpGet("type")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPaymentTransactionType([FromHeader] string authorization, [FromQuery] GetPaymentTransactionTypeRequest request)
        => await _paymentService.GetPaymentTransactionType(request);

    /// <summary>
    /// Search for scheduler types
    /// </summary>
    /// <param name="authorization">Token JWT</param>
    /// <param name="request">GetPaymentSchedulerRequest</param>
    /// <returns></returns>
    [HttpGet("scheduler")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPaymentScheduler([FromHeader] string authorization, [FromQuery] GetPaymentSchedulerRequest request)
        => await _paymentService.GetPaymentScheduler(request);

    /// <summary>
    /// Creates a new user payment transaction.
    /// </summary>
    /// <param name="authorization">Token JWT</param>
    /// <param name="request">PostPaymentTransactionRequest</param>
    /// <returns>IActionResult</returns>
    [HttpPost("")]
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

    /// <summary>
    /// Search for an user payment transaction.
    /// </summary>
    /// <param name="authorization">Token JWT</param>
    /// <param name="request">GetPaymentTransactionRequest</param>
    /// <returns>IActionResult</returns>
    [HttpGet("")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseApiSucess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPaymentTransaction([FromHeader] string authorization, [FromQuery] GetPaymentTransactionRequest request)
    {
        request.SetEmail(
            await _jsonWebTokenService.GetClaimValue(authorization, "email")
        );
        return await _paymentService.GetPaymentTransaction(request);
    }
}
