using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Services;

/// <summary>
/// Interface for payments services, handling payments operations.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Create a new user payment transaction
    /// </summary>
    /// <param name="request">PostPaymentTransactionRequest</param>
    Task<IActionResult> PostPaymentTransaction(PostPaymentTransactionRequest request);
}
