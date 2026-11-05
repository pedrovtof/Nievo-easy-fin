using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Data.Views;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
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
    private readonly UserModel _userModel;
    private readonly UserBankCardModel _userBankCardModel;
    private readonly CategoryModel _categoryModel;
    private readonly UserBankModel _userBankModel;

    public PaymentService(
        SchedulerTypeModel schedulerTypeModel,
        TransactionModel transactionModel,
        TransactionStatusModel transactionStatusModel,
        TransactionTypeModel transactionTypeModel,
        UserModel userModel,
        UserBankCardModel userBankCardModel,
        CategoryModel categoryModel,
        UserBankModel userBankModel
    )
    {
        _schedulerTypeModel = schedulerTypeModel;
        _transactionModel = transactionModel;
        _transactionStatusModel = transactionStatusModel;
        _transactionTypeModel = transactionTypeModel;
        _userModel = userModel;
        _userBankCardModel = userBankCardModel;
        _categoryModel = categoryModel;
        _userBankModel = userBankModel;
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

        var user = await _userModel.GetUserByEmailAsync(request.GetEmail());
        if (user == null)
            return NotFound(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_USER_NOT_FOUND.GetDescription() }
            ));

        var bank = await _userBankModel.GetUserBankByUserAndBankAsync(user.Id, request.Bank);
        if (bank == null)
            return NotFound(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_BANK_NOT_FOUND.GetDescription() }
            ));

        UserBankCardEntity? card = null;
        if (request.Card > 0)
        {
            card = await _userBankCardModel.GetUserBankCardByCardIdAndUserId(request.Card, user.Id);
            if (card == null)
                return NotFound(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_USER_CARD_NOT_FOUND.GetDescription() }
                ));
        }

        var transactionType = await _transactionTypeModel.GetTransactionTypeByIdAsync(request.TransactionType);
        if (transactionType == null)
            return NotFound(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_TRANSACTION_TYPE_NOT_FOUND.GetDescription() }
            ));

        var transactionStatus = await _transactionStatusModel.GetTransactionStatusByIdAsync(request.TransactionStatus);
        if (transactionStatus == null)
            return NotFound(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_TRANSACTION_STATUS_NOT_FOUND.GetDescription() }
            ));

        SchedulerTypeEntity? scheduler = null;
        if (request.Scheduler > 0)
        {
            scheduler = await _schedulerTypeModel.GetSchedulerTypeAsync(request.Scheduler);
            if (scheduler == null)
                return NotFound(new ResponseApiError(
                        new List<string>() { EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_SCHEDULER_NOT_FOUND.GetDescription() }
                    ));
        }

        var category = await _categoryModel.GetCategoryValidByNameAndUserId(request.CategoryName, user.Id);
        if (category == null)
            return NotFound(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_CATEGORY_NOT_FOUND.GetDescription() }
                ));

        var paymentTransaction = await _transactionModel.CreatePaymentTransaction(
            request.Name,
            request.Description,
            user.Id,
            transactionStatus.Id,
            transactionType.Id,
            category.Id,
            request.PayDate,
            request.Amount,
            request.Card > 0 ? card.Id : null,
            bank.Id,
            request.Scheduler > 0 ? scheduler.Id : null
        );

        return StatusCode(201, new ResponseApiSucess(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_201_SUCESS.GetDescription()));
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
