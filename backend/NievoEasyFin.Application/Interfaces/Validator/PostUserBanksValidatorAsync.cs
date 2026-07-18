using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using FluentValidation;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    /// <summary>
    /// Class validator for PostUserBankRequest
    /// </summary>
    public class PostUserBanksValidatorAsync : AbstractValidator<PostUserBanksRequest>
    {
        /// <summary>
        /// Default validator
        /// </summary>
        public PostUserBanksValidatorAsync()
        {
            RuleFor(x => x.BankId)
                .GreaterThanOrEqualTo(1)
                    .WithErrorCode(EnumErrosApi.POSTUSERBANKSASYNC_CORESERVICE_400_INVALID_BANK_ID.ToString());

            RuleFor(x => x.GetEmail())
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTUSERBANKSASYNC_CORESERVICE_400_EMPTY_EMAIL.ToString())
                .EmailAddress()
                    .WithErrorCode(EnumErrosApi.POSTUSERBANKSASYNC_CORESERVICE_400_INVALID_EMAIL.ToString());
        }
    }
}
