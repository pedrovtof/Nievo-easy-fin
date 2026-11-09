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

    /// <summary>
    /// Search for an user payment transaction
    /// </summary>
    /// <param name="request">GetPaymentTransactionRequest</param>
    Task<IActionResult> GetPaymentTransaction(GetPaymentTransactionRequest request);

    /// <summary>
    /// Search for transactions status
    /// </summary>
    /// <param name="request">GetPaymentTransactionStatusRequest</param>
    Task<IActionResult> GetPaymentTransactionStatus(GetPaymentTransactionStatusRequest request);

    /// <summary>
    ///  Search for transactions type
    /// </summary>
    /// <param name="request">GetPaymentTransactionTypeRequest</param>
    Task<IActionResult> GetPaymentTransactionType(GetPaymentTransactionTypeRequest request);

    /// <summary>
    /// Search for scheduler types
    /// </summary>
    /// <param name="request">GetPaymentSchedulerRequest</param>
    Task<IActionResult> GetPaymentScheduler(GetPaymentSchedulerRequest request);
}
