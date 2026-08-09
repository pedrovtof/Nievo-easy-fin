using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class BankCardTypeEntityFaker
{
    public static Faker<BankCardTypeEntity> Create()
    {
        return new Faker<BankCardTypeEntity>()
            .RuleFor(bct => bct.Id, f => f.Random.Int(1, 100000))
            .RuleFor(bct => bct.Name, f => f.Commerce.ProductName())
            .RuleFor(bct => bct.Description, f => f.Commerce.ProductDescription())
            .RuleFor(bct => bct.Active, true)
            .RuleFor(bct => bct.CreatedAt, f => f.Date.Recent())
            .RuleFor(bct => bct.UpdatedAt, f => f.Date.Recent());
    }
}
