using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class BankCardFlagEntityFaker
{
    public static Faker<BankCardFlagEntity> Create()
    {
        return new Faker<BankCardFlagEntity>()
            .RuleFor(bcf => bcf.Id, f => f.Random.Int(1, 100000))
            .RuleFor(bcf => bcf.Name, f => f.Commerce.ProductName())
            .RuleFor(bcf => bcf.Description, f => f.Commerce.ProductDescription())
            .RuleFor(bcf => bcf.Active, true)
            .RuleFor(bcf => bcf.CreatedAt, f => f.Date.Recent())
            .RuleFor(bcf => bcf.UpdatedAt, f => f.Date.Recent());
    }
}
