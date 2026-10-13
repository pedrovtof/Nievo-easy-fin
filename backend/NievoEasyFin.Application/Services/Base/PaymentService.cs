using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Interfaces.Validator;

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
        var validatorResult = await new PostPaymentTransactionValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        return Ok(new ResponseApiSucess(null));
    }

    /// <summary>
    ///  Search for transactions status
    /// </summary>
    /// <param name="request">GetPaymentTransactionStatusRequest</param>
    /// <returns>IActionResult</returns>
    public async Task<IActionResult> GetPaymentTransactionStatus(GetPaymentTransactionStatusRequest request)
    {
        var validatorResult = await new GetPaymentTransactionStatusValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        return Ok(new ResponseApiSucess(null));
    }

    /// <summary>
    /// Search for transactions type
    /// </summary>
    /// <param name="request">GetPaymentTransactionTypeRequest</param>
    /// <returns>IActionResult</returns>
    public async Task<IActionResult> GetPaymentTransactionType(GetPaymentTransactionTypeRequest request)
    {
        var validatorResult = await new GetPaymentTransactionTypeValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        return Ok(new ResponseApiSucess(null));
    }

    /// <summary>
    /// Search for scheduler
    /// </summary>
    /// <param name="request">GetPaymentSchedulerRequest</param>
    /// <returns>IActionResult</returns>
    public async Task<IActionResult> GetPaymentScheduler(GetPaymentSchedulerRequest request)
    {
        var validatorResult = await new GetPaymentSchedulerValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        return Ok(new ResponseApiSucess(null));
    }

    /// <summary>
    /// Search for an user payment transaction
    /// </summary>
    /// <param name="request">GetPaymentTransactionRequest</param>
    /// <returns>IActionResult</returns>
    public async Task<IActionResult> GetPaymentTransaction(GetPaymentTransactionRequest request)
    {
        var validatorResult = await new GetPaymentTransactionValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        return Ok(new ResponseApiSucess(null));
    }
}
