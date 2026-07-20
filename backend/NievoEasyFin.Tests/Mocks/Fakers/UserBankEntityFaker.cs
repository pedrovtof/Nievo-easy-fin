using Bogus;
using NievoEasyFin.Application.Data.Entities;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class UserBankEntityFaker
{
    public static Faker<UserBankEntity> Create()
    {
        return new Faker<UserBankEntity>()
            .RuleFor(u => u.Id, f => f.IndexFaker + 1)
            .RuleFor(u => u.NickName, f => f.Random.Word())
            .RuleFor(u => u.Active, true)
            .RuleFor(u => u.UserId, f => f.Random.Int(1, 1000))
            .RuleFor(u => u.BankId, f => f.Random.Int(1, 1000))
            .RuleFor(u => u.CreatedAt, f => f.Date.Recent())
            .RuleFor(u => u.UpdatedAt, f => f.Date.Recent());
    }
}
