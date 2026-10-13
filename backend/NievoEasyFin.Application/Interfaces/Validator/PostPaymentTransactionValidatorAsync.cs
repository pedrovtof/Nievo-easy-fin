using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    public class PostPaymentTransactionValidatorAsync : AbstractValidator<PostPaymentTransactionRequest>
    {
        public PostPaymentTransactionValidatorAsync()
        {

        }
    }
}