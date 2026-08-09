using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator;

public class GetCardTypeValidatorAsync : AbstractValidator<GetCardTypeRequest>
{
    public GetCardTypeValidatorAsync()
    {
        {
            RuleFor(x => x.Page)
                .Must(x => x > 0)
                    .WithErrorCode(EnumErrosApi.GETCARDTYPEASYNC_CORESERVICE_400_INVALID_PAGE.ToString());

            RuleFor(x => x.PageSize)
                .Must(x => x > 0 && x <= 50)
                    .WithErrorCode(EnumErrosApi.GETCARDTYPEASYNC_CORESERVICE_400_INVALID_PAGE_SIZE.ToString());
        }
    }
}