using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Interfaces.Validator;
using NievoEasyFin.Application.Models;

namespace NievoEasyFin.Application.Services.Base;

/// <summary>
/// Service responsible for payment operations and transactions.
/// </summary>
public class PaymentService : Controller, IPaymentService
{
    private readonly SchedulerTypeModel _schedulerTypeModel;
    private readonly TransactionModel _transactionModel;
    private readonly TransactionStatusModel _transactionStatusModel;
    private readonly TransactionTypeModel _transactionTypeModel;

    public PaymentService(
        SchedulerTypeModel schedulerTypeModel,
        TransactionModel transactionModel,
        TransactionStatusModel transactionStatusModel,
        TransactionTypeModel transactionTypeModel
    )
    {
        _schedulerTypeModel = schedulerTypeModel;
        _transactionModel = transactionModel;
        _transactionStatusModel = transactionStatusModel;
        _transactionTypeModel = transactionTypeModel;
    }

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

        var (transactionStatus, records) = await _transactionStatusModel.GetTransactionStatusAsync(request.Page, request.PageSize);

        List<GetPaymentTransactionStatusResponse> items = new();

        if (!transactionStatus.Any())
            return Ok(
                new ResponseApiSucess(new ResponsePaginationBase<GetPaymentTransactionStatusResponse>(
                request.Page,
                request.PageSize,
                0,
                items
                ))
            );

        var response = new ResponsePaginationBase<GetPaymentTransactionStatusResponse>(
            request.Page,
            request.PageSize,
            records,
            transactionStatus.Select(x => new GetPaymentTransactionStatusResponse(x)).ToList()
        );

        return Ok(new ResponseApiSucess(response));
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

        var (transactionType, records) = await _transactionTypeModel.GetTransactionTypeAsync(request.Page, request.PageSize);

        List<GetPaymentTransactionTypeResponse> items = new();

        if (!transactionType.Any())
            return Ok(
                new ResponseApiSucess(new ResponsePaginationBase<GetPaymentTransactionTypeResponse>(
                request.Page,
                request.PageSize,
                0,
                items
                ))
            );

        var response = new ResponsePaginationBase<GetPaymentTransactionTypeResponse>(
            request.Page,
            request.PageSize,
            records,
            transactionType.Select(x => new GetPaymentTransactionTypeResponse(x)).ToList()
        );

        return Ok(new ResponseApiSucess(response));
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

        var (schedulerType, records) = await _schedulerTypeModel.GetSchedulerTypeAsync(request.Page, request.PageSize);

        List<GetSchedulerTypeAsyncResponse> items = new();

        if (!schedulerType.Any())
            return Ok(
                new ResponseApiSucess(new ResponsePaginationBase<GetSchedulerTypeAsyncResponse>(
                request.Page,
                request.PageSize,
                0,
                items
                ))
            );

        var response = new ResponsePaginationBase<GetSchedulerTypeAsyncResponse>(
            request.Page,
            request.PageSize,
            records,
            schedulerType.Select(x => new GetSchedulerTypeAsyncResponse(x)).ToList()
        );

        return Ok(new ResponseApiSucess(response));
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
