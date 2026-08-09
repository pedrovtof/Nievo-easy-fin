using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for GetCardTypeRequest.
/// </summary>
public class GetCardTypeRequestBuilder : GetCardTypeRequest
{
    public GetCardTypeRequestBuilder()
    {
        Page = 1;
        PageSize = 10;
    }
}
