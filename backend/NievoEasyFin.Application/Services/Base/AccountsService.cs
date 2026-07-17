
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

        public AccountsService(
            BankModel bankModel,
            AuthDbCacheService authDbCacheService,
            BankTypeModel bankTypeModel
        )
        {
            _bankModel = bankModel;
            _authDbCacheService = authDbCacheService;
            _bankTypeModel = bankTypeModel;
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
    }
}
