using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class BankTypeEntityFaker
{
    public static Faker<BankTypeEntity> Create()
    {
        return new Faker<BankTypeEntity>()
            .RuleFor(u => u.Id, f => f.IndexFaker + 1)
            .RuleFor(u => u.Name, f => f.Company.CompanyName())
            .RuleFor(u => u.Description, f => f.Company.CatchPhrase())
            .RuleFor(u => u.Active, f => f.Random.Bool())
            .RuleFor(u => u.CreatedAt, f => f.Date.Recent());
    }
}
