using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class UserBankCardEntityFaker
{
    public static Faker<UserBankCardEntity> Create()
    {
        return new Faker<UserBankCardEntity>()
            .RuleFor(u => u.Id, f => f.IndexFaker + 1)
            .RuleFor(u => u.BankId, f => f.Random.Int(1, 1000))
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.CardId, f => f.Random.Int(1, 1000))
            .RuleFor(u => u.Active, true)
            .RuleFor(u => u.UserId, f => f.Random.Int(1, 1000))
            .RuleFor(u => u.ExpiredAt, f => f.Date.Future())
            .RuleFor(u => u.CreatedAt, f => f.Date.Recent())
            .RuleFor(u => u.UpdatedAt, f => f.Date.Recent());
    }
}
