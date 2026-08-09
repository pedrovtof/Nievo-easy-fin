using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class SsoProviderEntityFaker
{
    public static Faker<SsoProviderEntity> Create()
    {
        return new Faker<SsoProviderEntity>()
            .RuleFor(u => u.Id, f => f.IndexFaker + 1)
            .RuleFor(u => u.Name, f => f.Random.Word())
            .RuleFor(u => u.Active, f => f.Random.Bool());
    }
}
