using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Request;
using FluentValidation;
using NievoEasyfin.Application.Extensions.Enum;

namespace NievoEasyfin.Application.Interfaces.Validator;

/// <summary>
/// Class template for abstractorValidator in PatchResetPassword
/// </summary>
public class PostResetPasswordValidator : AbstractValidator<PostResetPasswordRequest>
{
    /// <summary>
    /// Constructor  to validate resquest PatchResetPassword
    /// </summary>
    public PostResetPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAIL.ToString())
            .EmailAddress()
                .WithErrorCode(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTY.ToString());
    }
}
