using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class UserProviderSsoEntityFaker
{
    public static Faker<UserProviderSsoEntity> Create()
    {
        return new Faker<UserProviderSsoEntity>()
            .RuleFor(u => u.SsoProviderId, f => f.Random.Int(1, 100))
            .RuleFor(u => u.UserId, f => f.Random.Int(1, 100))
            .RuleFor(u => u.Sub, f => f.Random.Guid().ToString());
    }
}
