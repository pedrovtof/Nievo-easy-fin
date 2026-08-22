using FluentValidation;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Application.Interfaces.Validator;

/// <summary>
/// Validator for GetBankCard
/// </summary>
public class GetBankCardValidatorAsync : AbstractValidator<GetBankCardRequest>
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public GetBankCardValidatorAsync()
    {
        RuleFor(x => x.Page)
            .Must(x => x > 0)
                .WithErrorCode(EnumErrosApi.GETBANKCARDASYNC_CORESERVICE_400_INVALID_PAGE.ToString());

        RuleFor(x => x.PageSize)
            .Must(x => x > 0 && x <= 100)
                .WithErrorCode(EnumErrosApi.GETBANKCARDASYNC_CORESERVICE_400_INVALID_PAGE.ToString());

        RuleFor(x => x.GetEmail())
            .NotEmpty()
                .WithErrorCode(EnumErrosApi.GETBANKCARDASYNC_CORESERVICE_400_EMPTY_EMAIL.ToString())
            .EmailAddress()
                .WithErrorCode(EnumErrosApi.GETBANKCARDASYNC_CORESERVICE_400_INVALID_EMAIL.ToString());

        RuleFor(x => x.BankId)
            .Must(x => Valid(x))
                .WithErrorCode(EnumErrosApi.GETBANKCARDASYNC_CORESERVICE_400_INVALID_BANK_ID.ToString());

        RuleFor(x => x.CardType)
            .Must(x => Valid(x))
                .WithErrorCode(EnumErrosApi.GETBANKCARDASYNC_CORESERVICE_400_INVALID_CARD_TYPE.ToString());

        RuleFor(x => x.Flag)
            .Must(x => Valid(x))
                .WithErrorCode(EnumErrosApi.GETBANKCARDASYNC_CORESERVICE_400_INVALID_CARD_FLAG.ToString());
    }

    /// <summary>
    /// Validate the optional param
    /// </summary>
    /// <param name="value">int</param>
    /// <returns>bool</returns>
    private bool Valid(int? value)
    {
        if (value == null)
            return true;
        else if (value <= 0)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Validate the optional param
    /// </summary>
    /// <param name="value">string</param>
    /// <returns>bool</returns>
    private bool Valid(string? value)
    {
        if (value == null)
            return true;
        else if (value.Length <= 0)
            return false;
        else
            return true;
    }
}
