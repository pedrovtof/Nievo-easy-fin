using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class TransactionTypeEntityFaker
{
    public static Faker<TransactionTypeEntity> Create()
    {
        return new Faker<TransactionTypeEntity>()
            .RuleFor(tt => tt.Id, f => f.Random.Int(1, 100000))
            .RuleFor(tt => tt.Name, f => f.Commerce.ProductName())
            .RuleFor(tt => tt.Description, f => f.Commerce.ProductDescription())
            .RuleFor(tt => tt.Active, true)
            .RuleFor(tt => tt.CreatedAt, f => f.Date.Recent())
            .RuleFor(tt => tt.UpdatedAt, f => f.Date.Recent());
    }
}
