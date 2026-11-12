using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    /// <summary>
    /// Validator for <see cref="GetPaymentTransactionSituationRequest"/> pagination parameters.
    /// </summary>
    public class GetPaymentTransactionSituationValidatorAsync : AbstractValidator<GetPaymentTransactionSituationRequest>
    {
        /// <summary>
        /// Initializes validation rules for transaction situation queries.
        /// </summary>
        public GetPaymentTransactionSituationValidatorAsync()
        {
            RuleFor(x => x.Page)
                .Must(x => x > 0)
                    .WithErrorCode(EnumErrosApi.GETPAYMENTTRANSACTIONSITUATION_CORESERVICE_400_INVALID_PAGE.ToString());

            RuleFor(x => x.PageSize)
                .Must(x => x > 0 && x <= 50)
                    .WithErrorCode(EnumErrosApi.GETPAYMENTTRANSACTIONSITUATION_CORESERVICE_400_INVALID_PAGESIZE.ToString());
        }
    }
}