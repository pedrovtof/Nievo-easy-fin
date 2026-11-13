using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator
{
    /// <summary>
    /// Validator for PostPaymentTransactionRequest payload rules.
    /// </summary>
    public class PostPaymentTransactionValidatorAsync : AbstractValidator<PostPaymentTransactionRequest>
    {
        /// <summary>
        /// Initializes validation rules for creating a payment transaction.
        /// </summary>
        public PostPaymentTransactionValidatorAsync()
        {
            RuleFor(x => x.GetEmail())
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_EMPTY_EMAIL.ToString())
                .EmailAddress()
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_INVALID_EMAIL.ToString());

            RuleFor(x => x.Card)
                .GreaterThanOrEqualTo(0)
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_INVALID_CARD.ToString());

            RuleFor(x => x.Bank)
                .GreaterThanOrEqualTo(0)
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_INVALID_BANK.ToString());

            RuleFor(x => x.TransactionStatus)
                .GreaterThan(0)
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_INVALID_TRANSACTION_STATUS.ToString());

            RuleFor(x => x.Amount)
                .GreaterThan(0.00m)
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_INVALID_AMOUNT.ToString());

            RuleFor(x => x.TransactionType)
                .GreaterThan(0)
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_INVALID_TRANSACTION_TYPE.ToString());

            RuleFor(x => x.Scheduler)
                .GreaterThanOrEqualTo(0)
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_INVALID_SCHEDULER.ToString());

            RuleFor(x => x.TransactionSituation)
                .GreaterThanOrEqualTo(1)
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_INVALID_SITUATION.ToString());

            RuleFor(x => x.Description)
                .NotNull()
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_INVALID_DESCRIPTION.ToString())
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_EMPTY_DESCRIPTION.ToString());

            RuleFor(x => x.Name)
                .NotNull()
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_INVALID_NAME.ToString())
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_EMPTY_NAME.ToString());

            RuleFor(x => x.CategoryName)
                .NotNull()
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_INVALID_CATEGORY.ToString())
                .NotEmpty()
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_EMPTY_CATEGORY.ToString());

            RuleFor(x => x.PayDate)
                .NotNull()
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_NULL_PAYDATE.ToString())
                .GreaterThan(new DateTime(1900, 1, 1))
                    .WithErrorCode(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_400_INVALID_PAYDATE.ToString());
        }
    }
}