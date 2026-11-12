using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for GetPaymentTransactionSituationRequest.
/// </summary>
public class GetPaymentTransactionSituationRequestBuilder : GetPaymentTransactionSituationRequest
{
    public GetPaymentTransactionSituationRequestBuilder()
    {
        Page = 1;
        PageSize = 10;
    }

    public GetPaymentTransactionSituationRequestBuilder WithPage(int page)
    {
        Page = page;
        return this;
    }

    public GetPaymentTransactionSituationRequestBuilder WithPageSize(int pageSize)
    {
        PageSize = pageSize;
        return this;
    }

    public GetPaymentTransactionSituationRequestBuilder WithInvalidPage()
    {
        Page = 0;
        return this;
    }

    public GetPaymentTransactionSituationRequestBuilder WithInvalidPageSize()
    {
        PageSize = 0;
        return this;
    }
}
