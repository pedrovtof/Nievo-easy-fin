using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class BankEntityFaker
{
    public static Faker<BankEntity> Create()
    {
        return new Faker<BankEntity>()
            .RuleFor(b => b.Id, f => f.IndexFaker + 1)
            .RuleFor(b => b.Name, f => f.Company.CompanyName())
            .RuleFor(b => b.BankType, f => f.Random.Int(1, 3))
            .RuleFor(b => b.Active, true)
            .RuleFor(b => b.CreatedAt, f => f.Date.Recent())
            .RuleFor(b => b.UpdatedAt, f => f.Date.Recent());
    }
}
