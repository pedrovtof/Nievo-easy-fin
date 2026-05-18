using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Helper;
using FluentValidation;

namespace NievoEasyFin.Application.Interfaces.Validator;

/// <summary>
/// Class template for abstractValidator in PostCreateUser
/// </summary>
public class PostCreateUserValidator : AbstractValidator<PostCreateUserRequest>
{
    private static PasswordValidatorHelper _passwordValidatorHelper = new PasswordValidatorHelper();
    /// <summary>
    /// Constructor for validate resquest in PostCreateUser
    /// </summary>
    public PostCreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_NAME_EMPTY_NULL.ToString())
            .Length(2, 100)
                .WithErrorCode(EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_NAME_WITH_WRONG_LENGHT.ToString());

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL.ToString())
            .Length(6, 12)
                .WithErrorCode(EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_PASSWORD_WITH_WRONG_LENGHT.ToString())
            .Must((x) => _passwordValidatorHelper.ValidatePasswordRegex(x))
                .WithErrorCode(EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_PASSWORD_WRONG_FORMAT.ToString());

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_EMPTY_NULL.ToString())
            .EmailAddress()
                .WithErrorCode(EnumErrosApi.POSTCREATEUSERASYNC_AUTHSERVICE_400_EMAIL_INVALID.ToString());
    }
}
