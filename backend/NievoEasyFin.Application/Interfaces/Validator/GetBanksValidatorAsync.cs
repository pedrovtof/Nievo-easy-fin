using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    /// <summary>
    /// Default validator for Get Banks
    /// </summary>
    public class GetBanksValidatorAsync : AbstractValidator<GetBanksRequest>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public GetBanksValidatorAsync()
        {
            RuleFor(x => x.Page)
                .Must(x => x > 0)
                    .WithErrorCode(EnumErrosApi.GETBANKSASYNC_AUTHSERVICE_400_INVALID_PAGE.ToString());

            RuleFor(x => x.PageSize)
                .Must(x => x > 0 && x <= 50)
                    .WithErrorCode(EnumErrosApi.GETBANKSASYNC_AUTHSERVICE_400_INVALID_PAGE_SIZE.ToString());
        }
    }
}
