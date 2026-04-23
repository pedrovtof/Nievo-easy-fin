using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Request;
using FluentValidation;

namespace NievoEasyfin.Application.Interfaces.Validator;

/// <summary>
/// Class template for abstractorValidator in PatchResetPassword
/// </summary>
public class PatchResetPasswordValidator : AbstractValidator<PatchResetPasswordRequest>
{
    /// <summary>
    /// Constructor  to validate resquest PatchResetPassword
    /// </summary>
    public PatchResetPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAIL.ToString())
            .EmailAddress()
                .WithErrorCode(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTY.ToString());

        RuleFor(x => x.PinToken)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAIL.ToString());
    }
}
