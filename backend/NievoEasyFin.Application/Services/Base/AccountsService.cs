
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

namespace NievoEasyFin.Application.Services.Base
{
    /// <summary>
    /// Service responsible for accounts management, including bank accounts operations.
    /// </summary>
    public class AccountsService : Controller, IAccountsService
    {
        private readonly BankModel _bankModel;

        private readonly AuthDbCacheService _authDbCacheService;

        private readonly BankTypeModel _bankTypeModel;

        private readonly UserModel _userModel;

        private readonly UserBankModel _userBankModel;

        private readonly BankCardModel _bankCardModel;

        private readonly BankCardTypeModel _bankCardTypeModel;

        public AccountsService(
            BankModel bankModel,
            AuthDbCacheService authDbCacheService,
            BankTypeModel bankTypeModel,
            UserModel userModel,
            UserBankModel userBankModel,
            BankCardModel bankCardModel,
            BankCardTypeModel bankCardTypeModel
        )
        {
            _bankModel = bankModel;
            _authDbCacheService = authDbCacheService;
            _bankTypeModel = bankTypeModel;
            _userModel = userModel;
            _userBankModel = userBankModel;
            _bankCardModel = bankCardModel;
            _bankCardTypeModel = bankCardTypeModel;
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
        /// Creates a new bank account for the authenticated user.
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
        /// Create a link between User and a Bank
        /// </summary>
        /// <param name="request">PostUserBanksRequest</param>
        /// <returns>IActionResult</returns>
        public async Task<IActionResult> PostUserBanks(PostUserBanksRequest request)
        {
            var validatorResult = await new PostUserBanksValidatorAsync().ValidateAsync(request);
            if (!validatorResult.IsValid)
                return BadRequest(
                   new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
               );

            var user = await _userModel.GetUserByEmailAsync(request.GetEmail());
            if (user == null)
                return NotFound(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTUSERBANKSASYNC_CORESERVICE_404_USER_NOT_FOUND.GetDescription() }
                ));

            var bank = await GetBankByNameAndTypeAsync(request.BankName, request.BankType);
            if (bank == null)
                return NotFound(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTUSERBANKSASYNC_CORESERVICE_400_BANK_NOT_FOUND.GetDescription() }
                ));

            var userBank = await _userBankModel.GetUserBankByUserAndBankAsync(user.Id, bank.Id);
            if (userBank != null)
                return BadRequest(new ResponseApiError(
                    new List<string>() { EnumErrosApi.POSTUSERBANKSASYNC_CORESERVICE_400_ALREADY_EXISTS_USER_BANK.GetDescription() }
                ));

            UserBankEntity newUserBank = await _userBankModel.CreateUserBankAsync(user.Id, request.NickName, bank.Id);

            return Ok(
                new ResponseApiSucess(EnumErrosApi.POSTUSERBANKSASYNC_CORESERVICE_200_CREATED.GetDescription())
            );
        }

        /// <summary>
        /// Get user bank accounts.
        /// </summary>
        /// <param name="request">GetUserBanksRequest</param>
        /// <returns>IActionResult</returns>
        public async Task<IActionResult> GetUserBanks(GetUserBanksRequest request)
        {
            var validatorResult = await new GetUserBanksValidatorAsync().ValidateAsync(request);
            if (!validatorResult.IsValid)
                return BadRequest(
                   new ResponseApiError(validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
               );

            var user = await _userModel.GetUserByEmailAsync(request.GetEmail());
            if (user == null)
                return NotFound(new ResponseApiError(
                    new List<string>() { EnumErrosApi.GETUSERBANKSASYNC_CORESERVICE_404_USER_NOT_FOUND.GetDescription() }
                ));

            var userBank = await _userBankModel.GetUserBankByUserAsync(user.Id);
            if (!userBank.Any())
                return Ok(
                    new ResponseApiSucess(userBank)
                );

            var response = userBank.Select(x => new GetUserBanksResponse(x)).ToList();

            return Ok(new ResponseApiSucess(response));
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

            var (cardTypes, itemsCount) = await _bankCardTypeModel.GetBankCardTypesAsync();
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
        /// Create an bank card
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

            var newCard = await _bankCardModel.CreateBankCardAsync(request.BankId, request.CardType, request.Name);

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

            var (items, itemsCount) = await _bankCardModel.GetBankCardsAsync(request.Page, request.PageSize, request.BankId, request.CardType);
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

        /// <summary>
        /// Get user card banks
        /// </summary>
        public async Task<IActionResult> GetUserCard(GetUserCardRequest request)
        {
            return Ok();
        }

        public async Task<IActionResult> PostUserCard(PostUserCardRequest request)
        {
            return Ok();
        }
    }
}
