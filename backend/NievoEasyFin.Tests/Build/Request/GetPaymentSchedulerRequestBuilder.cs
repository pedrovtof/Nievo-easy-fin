using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for GetPaymentSchedulerRequest.
/// </summary>
public class GetPaymentSchedulerRequestBuilder : GetPaymentSchedulerRequest
{
    public GetPaymentSchedulerRequestBuilder()
    {
        Page = 1;
        PageSize = 10;
    }

    public GetPaymentSchedulerRequestBuilder WithPage(int page)
    {
        Page = page;
        return this;
    }

    public GetPaymentSchedulerRequestBuilder WithPageSize(int pageSize)
    {
        PageSize = pageSize;
        return this;
    }

    public GetPaymentSchedulerRequestBuilder WithInvalidPage()
    {
        Page = 0;
        return this;
    }

    public GetPaymentSchedulerRequestBuilder WithInvalidPageSize()
    {
        PageSize = 0;
        return this;
    }
}
