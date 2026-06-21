
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Interfaces.Validator;

namespace NievoEasyFin.Application.Services.Base
{
    /// <summary>
    /// Service responsible for accounts management, including bank accounts operations.
    /// </summary>
    public class AccountsService : Controller, IAccountsService
    {
        public AccountsService()
        {

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

            return Ok(
                new ResponseApiSucess(EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_200_CREATED.GetDescription())
            );
        }
    }
}