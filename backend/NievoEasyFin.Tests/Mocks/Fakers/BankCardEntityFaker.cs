using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class BankCardEntityFaker
{
    public static Faker<BankCardEntity> Create()
    {
        return new Faker<BankCardEntity>()
            .RuleFor(bc => bc.Id, f => f.Random.Int(1, 100000))
            .RuleFor(bc => bc.BankId, f => f.Random.Int(1, 100))
            .RuleFor(bc => bc.Name, f => f.Commerce.ProductName())
            .RuleFor(bc => bc.CardType, f => f.Random.Int(1, 10))
            .RuleFor(bc => bc.Active, true)
            .RuleFor(bc => bc.CreatedAt, f => f.Date.Recent())
            .RuleFor(bc => bc.UpdatedAt, f => f.Date.Recent());
    }
}
