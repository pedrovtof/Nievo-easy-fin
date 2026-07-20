
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Data.Entities;
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

        public AccountsService(
            BankModel bankModel,
            AuthDbCacheService authDbCacheService,
            BankTypeModel bankTypeModel,
            UserModel userModel,
            UserBankModel userBankModel
        )
        {
            _bankModel = bankModel;
            _authDbCacheService = authDbCacheService;
            _bankTypeModel = bankTypeModel;
            _userModel = userModel;
            _userBankModel = userBankModel;
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
    }
}
