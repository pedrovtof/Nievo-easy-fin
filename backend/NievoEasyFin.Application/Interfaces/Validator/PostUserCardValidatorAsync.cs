using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;


namespace NievoEasyFin.Application.Interfaces.Validator
{
    public class PostUserCardValidatorAsync : AbstractValidator<PostUserCardRequest>
    {
        public PostUserCardValidatorAsync()
        {
            RuleFor(x => x.GetEmail())
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_INVALID_EMPTY_EMAIL.ToString())
                .EmailAddress()
                    .WithErrorCode(EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_INVALID_EMAIL.ToString());
        }
    }
}