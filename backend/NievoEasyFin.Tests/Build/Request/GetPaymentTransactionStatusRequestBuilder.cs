using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for GetPaymentTransactionStatusRequest.
/// </summary>
public class GetPaymentTransactionStatusRequestBuilder : GetPaymentTransactionStatusRequest
{
    public GetPaymentTransactionStatusRequestBuilder()
    {
        Page = 1;
        PageSize = 10;
    }

    public GetPaymentTransactionStatusRequestBuilder WithPage(int page)
    {
        Page = page;
        return this;
    }

    public GetPaymentTransactionStatusRequestBuilder WithPageSize(int pageSize)
    {
        PageSize = pageSize;
        return this;
    }

    public GetPaymentTransactionStatusRequestBuilder WithInvalidPage()
    {
        Page = 0;
        return this;
    }

    public GetPaymentTransactionStatusRequestBuilder WithInvalidPageSize()
    {
        PageSize = 0;
        return this;
    }
}
