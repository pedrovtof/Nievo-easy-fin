using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for GetBankCardRequest.
/// </summary>
public class GetBankCardRequestBuilder : GetBankCardRequest
{
    public GetBankCardRequestBuilder()
    {
        Page = 1;
        PageSize = 10;
        SetEmail(new Faker().Internet.Email());
    }

    public GetBankCardRequestBuilder WithInvalidEmail()
    {
        SetEmail("invalid-email");
        return this;
    }
}
