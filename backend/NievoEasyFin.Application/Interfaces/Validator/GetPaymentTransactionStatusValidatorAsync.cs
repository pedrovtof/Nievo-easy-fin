using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    public class GetPaymentTransactionStatusValidatorAsync : AbstractValidator<GetPaymentTransactionStatusRequest>
    {
        public GetPaymentTransactionStatusValidatorAsync()
        {
            RuleFor(x => x.Page)
                .Must(x => x > 0)
                    .WithErrorCode(EnumErrosApi.GETPAYMENTTRANSACTIONSTATUS_CORESERVICE_400_INVALID_PAGE.ToString());

            RuleFor(x => x.PageSize)
                .Must(x => x > 0 && x <= 50)
                    .WithErrorCode(EnumErrosApi.GETPAYMENTTRANSACTIONSTATUS_CORESERVICE_400_INVALID_PAGESIZE.ToString());
        }
    }
}