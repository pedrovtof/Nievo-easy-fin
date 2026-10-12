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
using NievoEasyFin.Application.Services.Cache;

namespace NievoEasyFin.Application.Services.Base;

/// <summary>
/// Service responsible for bank management, including bank accounts and bank cards operations.
/// </summary>
public class BankService : Controller, IBankService
{
    private readonly BankModel _bankModel;
    private readonly AuthDbCacheService _authDbCacheService;
    private readonly BankTypeModel _bankTypeModel;
    private readonly BankCardModel _bankCardModel;
    private readonly BankCardTypeModel _bankCardTypeModel;
    private readonly BankCardFlagModel _bankCardFlagModel;

    public BankService(
        BankModel bankModel,
        AuthDbCacheService authDbCacheService,
        BankTypeModel bankTypeModel,
        BankCardModel bankCardModel,
        BankCardTypeModel bankCardTypeModel,
        BankCardFlagModel bankCardFlagModel)
    {
        _bankModel = bankModel;
        _authDbCacheService = authDbCacheService;
        _bankTypeModel = bankTypeModel;
        _bankCardModel = bankCardModel;
        _bankCardTypeModel = bankCardTypeModel;
        _bankCardFlagModel = bankCardFlagModel;
    }

    /// <summary>
    /// Search the entity bank in the redis or database
    /// may create a value in redis 
    /// </summary>
    /// <param name="name">Bank name</param>
    /// <param name="bankType">Bank type</param>
    /// <returns>BankEntity</returns>
    private async Task<BankEntity> GetBankByNameAndTypeAsync(string name, int bankType)
    {
        var resultCache = await _authDbCacheService.GetBankByNameAndTypeAsync(name, bankType);
        if (resultCache != null)
            return resultCache;

        var resultDatabase = await _bankModel.GetBankByNameAndTypeAsync(name, bankType);
        if (resultDatabase != null)
            await _authDbCacheService.CreateBankAsync(resultDatabase);

        return resultDatabase;
    }

    /// <summary>
    /// Search the entity bank type in the redis or database
    /// may create a value in redis 
    /// </summary>
    /// <param name="id">int</param>
    /// <returns>BankTypeEntity</returns>
    private async Task<BankTypeEntity> GetBankTypeByIdAsync(int id)
    {
        var resultCache = await _authDbCacheService.GetBankTypeByIdAsync(id);
        if (resultCache != null)
            return resultCache;

        var resultDatabase = await _bankTypeModel.GetBankTypeByNameAsync(id);
        if (resultDatabase != null)
            await _authDbCacheService.CreateBankTypeAsync(resultDatabase);

        return resultDatabase;
    }

    /// <summary>
    /// Creates a new bank account.
    /// </summary>
    /// <param name="request">The bank account creation request data.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the bank account creation.</returns>
    public async Task<IActionResult> PostAccountsBanks(PostAccountsBanksRequest request)
    {
        var validatorResult = await new PostAccountsBanksValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var bank = await GetBankByNameAndTypeAsync(request.Name, request.BankType);
        if (bank != null)
            return BadRequest(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_400_BANK_ALREADY_EXISTS.GetDescription() }
            ));

        var bankType = await GetBankTypeByIdAsync(request.BankType);
        if (bankType == null)
            return BadRequest(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_400_BANKTYPE_INVALID.GetDescription() }
            ));

        BankEntity bankEntity = await _bankModel.CreateBankAsync(request.Name, request.BankType);

        return Ok(
            new ResponseApiSucess(EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_200_CREATED.GetDescription())
        );
    }

    /// <summary>
    /// Get banks list
    /// </summary>
    /// <param name="request"></param>
    /// <returns>IActionResult</returns>
    public async Task<IActionResult> GetBanks(GetBanksRequest request)
    {
        var validatorResult = await new GetBanksValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var (banks, records) = await _bankModel.GetBanksAsync(request.Page, request.PageSize);

        List<GetBanksResponse> items = new();

        if (!banks.Any())
        {
            return Ok(
                new ResponseApiSucess(new ResponsePaginationBase<GetBanksResponse>(
                request.Page,
                request.PageSize,
                0,
                items
                ))
            );
        }

        var response = new ResponsePaginationBase<GetBanksResponse>(
            request.Page,
            request.PageSize,
            records,
            banks.Select(x => new GetBanksResponse(x)).ToList()
        );

        return Ok(new ResponseApiSucess(response));
    }

    /// <summary>
    /// Create a bank card
    /// </summary>
    /// <param name="request">PostBankCardRequest</param>
    public async Task<IActionResult> PostBankCard(PostBankCardRequest request)
    {
        var validatorResult = await new PostBankCardValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
           );

        var bank = await _bankModel.GetBankByIdAsync(request.BankId);
        if (bank == null)
            return NotFound(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_404_BANK_NOT_FOUND.GetDescription() }
            ));

        var cardType = await _bankCardTypeModel.GetBankCardTypeByIdOrNameAsync(request.CardType, request.Name);
        if (cardType == null)
            return NotFound(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_404_CARD_TYPE_NOT_FOUND.GetDescription() }
            ));

        var card = await _bankCardModel.GetBankCardByBankIdAndCardTypeAndNameAsync(request.BankId, request.CardType, request.Name);
        if (card != null)
            return BadRequest(new ResponseApiError(
                new List<string>() { EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_400_CARD_ALREADY_EXISTS.GetDescription() }
            ));

        var flag = await _bankCardFlagModel.GetBankCardFlagByName(request.Flag);
        if (flag == null)
            return NotFound(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_404_FLAG_NOT_FOUND.GetDescription() }
                ));

        var newCard = await _bankCardModel.CreateBankCardAsync(request.BankId, request.CardType, request.Name, flag.Id);

        return Ok(new ResponseApiSucess(
            EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_200_CARD_CREATED.GetDescription()
        ));
    }

    /// <summary>
    /// Search for bank cards
    /// </summary>
    /// <param name="request">GetBankCardRequest</param>
    public async Task<IActionResult> GetBankCard(GetBankCardRequest request)
    {
        var validatorResult = await new GetBankCardValidatorAsync().ValidateAsync(request);
        if (!validatorResult.IsValid)
            return BadRequest(
                new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            );

        var (items, itemsCount) = await _bankCardModel.GetBankCardsAsync(request.Page, request.PageSize, request.BankId, request.CardType, request.Flag);
        if (!items.Any())
        {
            return Ok(
                new ResponseApiSucess(new ResponsePaginationBase<BankCardView>(
                    request.Page,
                    request.PageSize,
                    0,
                    new()
                ))
            );
        }

        GetBankCardResponse response = new(
            request.Page,
            request.PageSize,
            itemsCount,
            items
        );

        return Ok(new ResponseApiSucess(response));
    }
}
