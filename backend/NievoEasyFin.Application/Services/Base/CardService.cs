using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Data.Views;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Interfaces.Validator;
using NievoEasyFin.Application.Models;

namespace NievoEasyFin.Application.Services.Base;

/// <summary>
/// Service responsible for card management, including card types and card flags operations.
/// </summary>
public class CardService : Controller, ICardService
{
    private readonly BankCardTypeModel _bankCardTypeModel;
    private readonly BankCardFlagModel _bankCardFlagModel;

    public CardService(
        BankCardTypeModel bankCardTypeModel,
        BankCardFlagModel bankCardFlagModel)
    {
        _bankCardTypeModel = bankCardTypeModel;
        _bankCardFlagModel = bankCardFlagModel;
    }

    /// <summary>
    /// Get card types
    /// </summary>
    /// <param name="request">GetCardTypeRequest</param>
    /// <returns></returns>
    public async Task<IActionResult> GetCardType(GetCardTypeRequest request)
    {
        var validatorResult = await new GetCardTypeValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var (cardTypes, itemsCount) = await _bankCardTypeModel.GetBankCardTypesAsync(request.Page, request.PageSize);
        if (!cardTypes.Any())
        {
            return Ok(
                new ResponseApiSucess(new ResponsePaginationBase<BankCardTypeView>(
                    request.Page,
                    request.PageSize,
                    0,
                    new()
                ))
            );
        }

        var cardTypeView = cardTypes.Select(x => new BankCardTypeView(x)).ToList();

        GetCardTypeResponse response = new(
            request.Page,
            request.PageSize,
            itemsCount,
            cardTypeView
        );

        return Ok(new ResponseApiSucess(response));
    }

    /// <summary>
    /// Search for valid flag cards
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<IActionResult> GetCardFlag(GetCardFlagRequest request)
    {
        var validatorResult = await new GetCardFlagValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var (cardFlag, itemsCount) = await _bankCardFlagModel.GetBankCardFlagAsync(request.Page, request.PageSize);
        if (!cardFlag.Any())
        {
            return Ok(
                new ResponseApiSucess(new ResponsePaginationBase<BankCardFlagView>(
                    request.Page,
                    request.PageSize,
                    0,
                    new()
                ))
            );
        }

        var cardFlagView = cardFlag.Select(x => new BankCardFlagView(x)).ToList();

        GetCardFlagesponse response = new(
            request.Page,
            request.PageSize,
            itemsCount,
            cardFlagView
        );

        return Ok(new ResponseApiSucess(response));
    }
}
