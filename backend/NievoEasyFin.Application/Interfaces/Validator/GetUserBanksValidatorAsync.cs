using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    public class GetUserBanksValidatorAsync : AbstractValidator<GetUserBanksRequest>
    {
        public GetUserBanksValidatorAsync()
        {
            RuleFor(x => x.GetEmail())
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.GETUSERBANKSASYNC_CORESERVICE_400_EMPTY_EMAIL.ToString())
                .EmailAddress()
                    .WithErrorCode(EnumErrosApi.GETUSERBANKSASYNC_CORESERVICE_400_INVALID_EMAIL.ToString());
        }
    }
}
