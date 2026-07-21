using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class GetBanksRequestFaker
{
    public static Faker<GetBanksRequest> Create()
    {
        return new Faker<GetBanksRequest>()
            .RuleFor(r => r.Page, f => f.Random.Int(1, 10))
            .RuleFor(r => r.PageSize, f => f.Random.Int(1, 50));
    }
}
