using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for GetCardFlagRequest.
/// </summary>
public class GetCardFlagRequestBuilder : GetCardFlagRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public GetCardFlagRequestBuilder()
    {
        Page = 1;
        PageSize = 10;
    }
}
