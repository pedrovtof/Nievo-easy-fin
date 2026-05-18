using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostCreateUserSsoRequest.
/// </summary>
public class PostCreateUserSsoRequestBuilder : PostCreateUserSsoRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostCreateUserSsoRequestBuilder()
    {
        Provider = _faker.PickRandom("google", "facebook", "github");
        ProviderAccessToken = _faker.Random.AlphaNumeric(32);
    }

    public PostCreateUserSsoRequestBuilder WithProvider(string provider)
    {
        Provider = provider;
        return this;
    }

    public PostCreateUserSsoRequestBuilder WithProviderAccessToken(string token)
    {
        ProviderAccessToken = token;
        return this;
    }
}
