using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    /// <summary>
    /// FluentValidation validator for <see cref="GetUserDetailsRequest"/>.
    /// </summary>
    public class GetUserDetailsValidatorAsync : AbstractValidator<GetUserDetailsRequest>
    {
        /// <summary>
        /// Initializes validation rules for the user's email claim.
        /// </summary>
        public GetUserDetailsValidatorAsync()
        {
            RuleFor(x => x.GetEmail())
               .NotEmpty()
                   .WithErrorCode(EnumErrosApi.GETUSERDETAILSYASYNC_CORESERVICE_400_EMPTY_EMAIL.ToString())
               .EmailAddress()
                   .WithErrorCode(EnumErrosApi.GETUSERDETAILSYASYNC_CORESERVICE_400_INVALID_EMAIL.ToString());
        }
    }
}