using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class PostAccountsBanksRequestFaker
{
    public static Faker<PostAccountsBanksRequest> Create()
    {
        return new Faker<PostAccountsBanksRequest>()
            .RuleFor(r => r.Name, f => f.Company.CompanyName())
            .RuleFor(r => r.BankType, f => f.Random.Int(1, 10));
    }
}
