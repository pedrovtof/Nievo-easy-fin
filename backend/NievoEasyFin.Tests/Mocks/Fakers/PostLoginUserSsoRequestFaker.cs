using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class PostLoginUserSsoRequestFaker
{
    public static Faker<PostLogiPostLoginUserSsoRequest> Create()
    {
        return new Faker<PostLogiPostLoginUserSsoRequest>()
            .RuleFor(r => r.Provider, "google")
            .RuleFor(r => r.ProviderAccessToken, f => f.Random.AlphaNumeric(32));
    }
}
