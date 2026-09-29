using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    public class PostUserGoalValidatorAsync : AbstractValidator<PostUserGoalRequest>
    {
        public PostUserGoalValidatorAsync()
        {
            RuleFor(x => x.GetEmail())
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTUSERGOALASYNC_CORESERVICE_400_EMPTY_EMAIL.ToString())
                .EmailAddress()
                    .WithErrorCode(EnumErrosApi.POSTUSERGOALASYNC_CORESERVICE_400_INVALID_EMAIL.ToString());

            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTUSERGOALASYNC_CORESERVICE_400_EMPTY_NAME.ToString())
                .Must(x => x.Length > 1 && x.Length < 100)
                    .WithErrorCode(EnumErrosApi.POSTUSERGOALASYNC_CORESERVICE_400_INVALID_NAME.ToString());

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                    .WithErrorCode(EnumErrosApi.POSTUSERGOALASYNC_CORESERVICE_400_INVALID_AMOUNT.ToString());

            RuleFor(x => x.ExpireAt)
                .NotEmpty()
                .NotNull()
                .Must(x => ValidateExpiredAt(x))
                    .WithErrorCode(EnumErrosApi.POSTUSERGOALASYNC_CORESERVICE_400_INVALID_EXPIRED_AT.ToString());
        }

        /// <summary>
        /// Validate the expired date
        /// </summary>
        /// <param name="x">Datetime</param>
        /// <returns>bool</returns>
        private bool ValidateExpiredAt(DateTime x)
            => x.Date > DateTime.Today;
    }
}
