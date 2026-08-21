using System.Data;
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

            RuleFor(x => x.BankId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                    .WithErrorCode(EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_INVALID_BANK.ToString());

            RuleFor(x => x.CardId)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                    .WithErrorCode(EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_INVALID_CARD.ToString());

            RuleFor(x => x.CardUserName)
                .NotEmpty()
                .NotNull()
                .WithErrorCode(EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_INVALID_CARDNAME.ToString());

            RuleFor(x => x.ExpireAt)
                .NotEmpty()
                .NotNull()
                .WithErrorCode(EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_INVALID_EXPIREDAT.ToString());
        }
    }
}