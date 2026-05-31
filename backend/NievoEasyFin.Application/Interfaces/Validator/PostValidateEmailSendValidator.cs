using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using FluentValidation;

namespace NievoEasyFin.Application.Interfaces.Validator;

/// <summary>
/// Class template for abstractorValidator in PostValidateEmailSend
/// </summary>
public class PostValidateEmailSendValidator : AbstractValidator<PostValidateEmailSendRequest>
{
    public PostValidateEmailSendValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_400_EMPTY_EMAIL.ToString())
            .EmailAddress()
                .WithErrorCode(EnumErrosApi.POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_400_INVALID_EMAIL.ToString());
    }
}
