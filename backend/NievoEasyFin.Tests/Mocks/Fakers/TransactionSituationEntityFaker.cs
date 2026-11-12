using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

/// <summary>
/// Faker generator for TransactionSituationEntity instances.
/// </summary>
public static class TransactionSituationEntityFaker
{
    public static Faker<TransactionSituationEntity> Create()
    {
        return new Faker<TransactionSituationEntity>()
            .RuleFor(ts => ts.Id, f => f.Random.Int(1, 100000))
            .RuleFor(ts => ts.Name, f => f.PickRandom("Entrada", "Saida"))
            .RuleFor(ts => ts.Description, f => f.Commerce.ProductDescription())
            .RuleFor(ts => ts.Active, true)
            .RuleFor(ts => ts.CreatedAt, f => f.Date.Recent())
            .RuleFor(ts => ts.UpdatedAt, f => f.Date.Recent());
    }
}
