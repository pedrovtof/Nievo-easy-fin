using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using FluentValidation;

namespace NievoEasyFin.Application.Interfaces.Validator;

/// <summary>
/// Class template for abstractorValidator in PostResetPassword
/// </summary>
public class PostResetPasswordValidator : AbstractValidator<PostResetPasswordRequest>
{
    /// <summary>
    /// Constructor  to validate resquest PostResetPassword
    /// </summary>
    public PostResetPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAIL.ToString())
            .EmailAddress()
                .WithErrorCode(EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTY.ToString());
    }
}
