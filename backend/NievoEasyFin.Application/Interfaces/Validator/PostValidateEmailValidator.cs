using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using FluentValidation;

namespace NievoEasyFin.Application.Interfaces.Validator;

/// <summary>
/// Class template for abstractorValidator in PostValidateEmailRequest
/// </summary>
public class PostValidateEmailValidator : AbstractValidator<PostValidateEmailRequest>
{
    public PostValidateEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_400_EMPTY_EMAIL.ToString())
            .EmailAddress()
                .WithErrorCode(EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_400_INVALID_EMAIL.ToString());

        RuleFor(x => x.PinToken)
            .Must(x => x > 0)
                .WithErrorCode(EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_400_INVALID_TOKEN.ToString());
    }
}
