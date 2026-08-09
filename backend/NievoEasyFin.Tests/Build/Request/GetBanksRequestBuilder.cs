using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for GetBanksRequest.
/// </summary>
public class GetBanksRequestBuilder : GetBanksRequest
{
    public GetBanksRequestBuilder()
    {
        Page = 1;
        PageSize = 10;
    }
}
