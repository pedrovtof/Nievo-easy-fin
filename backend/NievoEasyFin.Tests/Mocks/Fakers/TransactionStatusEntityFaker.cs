using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class TransactionStatusEntityFaker
{
    public static Faker<TransactionStatusEntity> Create()
    {
        return new Faker<TransactionStatusEntity>()
            .RuleFor(ts => ts.Id, f => f.Random.Int(1, 100000))
            .RuleFor(ts => ts.Name, f => f.Commerce.ProductName())
            .RuleFor(ts => ts.Description, f => f.Commerce.ProductDescription())
            .RuleFor(ts => ts.Active, true)
            .RuleFor(ts => ts.CreatedAt, f => f.Date.Recent())
            .RuleFor(ts => ts.UpdatedAt, f => f.Date.Recent());
    }
}
