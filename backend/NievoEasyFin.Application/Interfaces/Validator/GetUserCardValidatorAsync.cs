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

            RuleFor(x => x.BankId)
                .Must(x => Valid(x))
                .GreaterThan(0)
                    .WithErrorCode(EnumErrosApi.GETUSERCARDASYNC_CORESERVICE_400_INVALID_BANK_ID.ToString());

            RuleFor(x => x.Flag)
                .Must(x => Valid(x))
                    .WithErrorCode(EnumErrosApi.GETUSERCARDASYNC_CORESERVICE_400_INVALID_FLAG.ToString());
        }

        /// <summary>
        /// Validate the bankId
        /// </summary>
        /// <param name="x">int</param>
        /// <returns>true/false</returns>
        private bool Valid(int? x)
        {
            if (x == null)
                return true;
            else if (x > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Validate the Flag
        /// </summary>
        /// <param name="x">string</param>
        /// <returns>true/false</returns>
        private bool Valid(string? x)
        {
            if (x == null)
                return true;
            else if (x.Length <= 0)
                return false;
            else
                return true;
        }
    }
}
