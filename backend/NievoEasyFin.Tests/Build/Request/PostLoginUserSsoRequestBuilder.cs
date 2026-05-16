using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostLogiPostLoginUserSsoRequest.
/// Default values are set in the constructor — no need to call Default().
/// </summary>
public class PostLoginUserSsoRequestBuilder : PostLogiPostLoginUserSsoRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostLoginUserSsoRequestBuilder()
    {
        Provider = "google";
        ProviderAccessToken = _faker.Random.AlphaNumeric(100);
    }

    public PostLoginUserSsoRequestBuilder WithProvider(string provider)
    {
        Provider = provider;
        return this;
    }

    public PostLoginUserSsoRequestBuilder WithProviderAccessToken(string providerAccessToken)
    {
        ProviderAccessToken = providerAccessToken;
        return this;
    }
}
