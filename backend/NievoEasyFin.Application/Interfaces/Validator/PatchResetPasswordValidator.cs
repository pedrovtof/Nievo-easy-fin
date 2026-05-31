using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using FluentValidation;
using NievoEasyFin.Application.Helper;

namespace NievoEasyFin.Application.Interfaces.Validator;

/// <summary>
/// Class template for abstractorValidator in PatchResetPassword
/// </summary>
public class PatchResetPasswordValidator : AbstractValidator<PatchResetPasswordRequest>
{
    private static PasswordValidatorHelper _passwordValidatorHelper = new PasswordValidatorHelper();

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
                .WithErrorCode(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_TOKEN_FORMAT.ToString());

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL.ToString())
            .Length(6, 12)
                .WithErrorCode(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_WITH_WRONG_LENGHT.ToString())
            .Must((x) => _passwordValidatorHelper.ValidatePasswordRegex(x))
                .WithErrorCode(EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_WRONG_FORMAT.ToString());
    }
}
