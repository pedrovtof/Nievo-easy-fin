using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;

namespace NievoEasyFin.Application.Services.Base;

/// <summary>
/// Service responsible for payment operations and transactions.
/// </summary>
public class PaymentService : Controller, IPaymentService
{
    /// <summary>
    /// Post Payment transaction
    /// </summary>
    /// <param name="request">PostPaymentTransactionRequest</param>
    /// <returns>IActionResult</returns>
    public async Task<IActionResult> PostPaymentTransaction(PostPaymentTransactionRequest request)
    {
        return await Task.FromResult(Ok(new ResponseApiSucess(null)));
    }
}
