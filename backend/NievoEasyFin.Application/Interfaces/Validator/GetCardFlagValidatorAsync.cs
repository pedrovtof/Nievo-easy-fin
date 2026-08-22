using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    /// <summary>
    /// Validator card flag get
    /// </summary>
    public class GetCardFlagValidatorAsync : AbstractValidator<GetCardFlagRequest>
    {
        public GetCardFlagValidatorAsync()
        {
            RuleFor(x => x.Page)
                .Must(x => x > 0)
                    .WithErrorCode(EnumErrosApi.GETCARDFLAGPEASYNC_CORESERVICE_400_INVALID_PAGE.ToString());

            RuleFor(x => x.PageSize)
                .Must(x => x > 0 && x <= 50)
                    .WithErrorCode(EnumErrosApi.GETCARDFLAGPEASYNC_CORESERVICE_400_INVALID_PAGE_SIZE.ToString());
        }
    }
}
