using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;


namespace NievoEasyFin.Application.Interfaces.Services
{
    public class GetUserCardValidatorAsync : AbstractValidator<GetUserCardRequest>
    {
        public GetUserCardValidatorAsync()
        {
            RuleFor(x => x.Page)
                .Must(x => x > 0)
                .WithErrorCode(EnumErrosApi.GETUSERCARDASYNC_CORESERVICE_400_INVALID_PAGE.ToString());

            RuleFor(x => x.PageSize)
                .Must(x => x > 0 && x < 101)
                .WithErrorCode(EnumErrosApi.GETUSERCARDASYNC_CORESERVICE_400_INVALID_PAGE_SIZE.ToString());
        }
    }
}