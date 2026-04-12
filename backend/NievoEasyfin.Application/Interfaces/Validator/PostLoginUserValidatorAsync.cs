using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Request;
using FluentValidation;

namespace NievoEasyfin.Application.Interfaces.Validator
{
    /// <summary>
    /// Class template for abstractorValidator in PostLoginUserRequest
    /// </summary>
    public class PostLoginUserValidatorAsync : AbstractValidator<PostLoginUserRequest>
    {
        /// <summary>
        /// Constructor to validate resquest in PostLoginUser
        /// </summary>
        public PostLoginUserValidatorAsync()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_EMAIL_EMPTY_NULL.ToString());

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTLOGINUSERASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL.ToString());
        }
    }
}
