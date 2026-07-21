using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    /// <summary>
    /// Class template for abstractValidator in PostAccountsBanksRequest
    /// </summary>
    public class PostAccountsBanksValidatorAsync : AbstractValidator<PostAccountsBanksRequest>
    {
        /// <summary>
        /// Constructor to validate request in PostAccountsBanks
        /// </summary>
        public PostAccountsBanksValidatorAsync()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_400_EMPTY_NAME.ToString());

            RuleFor(x => x.BankType)
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_400_EMPTY_BANKTYPE.ToString())
                .GreaterThanOrEqualTo(1)
                    .WithErrorCode(EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_400_LESS_THAN_0_BANKTYPE.ToString());
        }
    }
}
