using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class TransactionEntityFaker
{
    public static Faker<TransactionEntity> Create()
    {
        return new Faker<TransactionEntity>()
            .RuleFor(t => t.Id, f => f.Random.Guid())
            .RuleFor(t => t.Name, f => f.Commerce.ProductName())
            .RuleFor(t => t.Description, f => f.Commerce.ProductDescription())
            .RuleFor(t => t.UserId, f => f.Random.Int(1, 10000))
            .RuleFor(t => t.TransactionStatusId, f => f.Random.Int(1, 10))
            .RuleFor(t => t.CategorieId, f => f.Random.Int(1, 10))
            .RuleFor(t => t.PayDate, f => f.Date.Recent())
            .RuleFor(t => t.Amount, f => f.Random.Long(100, 100000))
            .RuleFor(t => t.CardId, f => f.Random.Int(1, 10))
            .RuleFor(t => t.BankId, f => f.Random.Int(1, 10))
            .RuleFor(t => t.TransactionTypeId, f => f.Random.Int(1, 10))
            .RuleFor(t => t.SchedulerId, f => f.Random.Int(1, 10))
            .RuleFor(t => t.CreatedAt, f => f.Date.Recent())
            .RuleFor(t => t.UpdatedAt, f => f.Date.Recent());
    }
}
