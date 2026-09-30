using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    public class GetUserCategoryValidatorAsync : AbstractValidator<GetUserCategoryRequest>
    {
        public GetUserCategoryValidatorAsync()
        {
            RuleFor(x => x.Page)
                .Must(x => x > 0)
                    .WithErrorCode(EnumErrosApi.GETUSERCATEGORYASYNC_CORESERVICE_400_INVALID_PAGE.ToString());

            RuleFor(x => x.PageSize)
                .Must(x => x > 0 && x <= 50)
                    .WithErrorCode(EnumErrosApi.GETUSERCATEGORYASYNC_CORESERVICE_400_INVALID_PAGESIZE.ToString());
        }
    }
}
