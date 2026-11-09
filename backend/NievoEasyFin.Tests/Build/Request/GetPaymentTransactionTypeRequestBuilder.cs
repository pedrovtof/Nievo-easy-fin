using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for GetPaymentTransactionTypeRequest.
/// </summary>
public class GetPaymentTransactionTypeRequestBuilder : GetPaymentTransactionTypeRequest
{
    public GetPaymentTransactionTypeRequestBuilder()
    {
        Page = 1;
        PageSize = 10;
    }

    public GetPaymentTransactionTypeRequestBuilder WithPage(int page)
    {
        Page = page;
        return this;
    }

    public GetPaymentTransactionTypeRequestBuilder WithPageSize(int pageSize)
    {
        PageSize = pageSize;
        return this;
    }

    public GetPaymentTransactionTypeRequestBuilder WithInvalidPage()
    {
        Page = 0;
        return this;
    }

    public GetPaymentTransactionTypeRequestBuilder WithInvalidPageSize()
    {
        PageSize = 0;
        return this;
    }
}
