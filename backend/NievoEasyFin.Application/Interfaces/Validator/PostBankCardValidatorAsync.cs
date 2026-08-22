using FluentValidation;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    /// <summary>
    /// Class validator for PostBankCardRequest
    /// </summary>
    public class PostBankCardValidatorAsync : AbstractValidator<PostBankCardRequest>
    {
        /// <summary>
        /// Default validator
        /// </summary>
        public PostBankCardValidatorAsync()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_400_EMPTY_NAME.ToString())
                .Must(x => x.Length > 1 && x.Length <= 100)
                    .WithErrorCode(EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_400_INVALID_NAME.GetDescription());

            RuleFor(x => x.Flag)
                .NotEmpty()
                .NotNull()
                .WithErrorCode(EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_400_INVALID_FLAG.ToString());

            RuleFor(x => x.BankId)
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_400_EMPTY_BANK_ID.ToString())
                .GreaterThanOrEqualTo(1)
                    .WithErrorCode(EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_400_INVALID_BANK_ID.ToString());

            RuleFor(x => x.CardType)
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_400_EMPTY_CARD_TYPE.ToString())
                .GreaterThanOrEqualTo(1)
                    .WithErrorCode(EnumErrosApi.POSTBANKCARDASYNC_CORESERVICE_400_INVALID_CARD_TYPE.ToString());
        }
    }
}
