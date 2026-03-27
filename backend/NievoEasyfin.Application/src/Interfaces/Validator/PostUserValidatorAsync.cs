using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Request;
using FluentValidation;

namespace NievoEasyfin.Application.Interfaces.Validator
{
    public class PostUserValidator : AbstractValidator<PostUserRequest>
    {
        public PostUserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("O nome deve ser preenchido.")
                .Length(3, 100)
                    .WithMessage("O nome deve ter entre 3 e 100 caracteres.")
                .WithErrorCode(EnumErrosApi.POSTUSERASYNC_AUTHSERVICE_400_INVALID_NAME_INPUT.ToString());

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("A senha deve ser preenchida.")
                .Length(6, 12)
                    .WithMessage("A senha deve ter entre 6 e 12 caracteres.")
                .WithErrorCode(EnumErrosApi.POSTUSERASYNC_AUTHSERVICE_400_INVALID_PASSWORD_INPUT.ToString());

            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithMessage("O email deve ser preenchido.")
                .EmailAddress()
                    .WithMessage("O email deve ser válido.")
                .WithErrorCode(EnumErrosApi.POSTUSERASYNC_AUTHSERVICE_400_INVALID_EMAIL_INPUT.ToString());
        }
    }
}