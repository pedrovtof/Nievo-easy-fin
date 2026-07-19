using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class PostUserBanksRequestFaker
{
    public static Faker<PostUserBanksRequest> Create()
    {
        return new Faker<PostUserBanksRequest>()
            .RuleFor(r => r.BankName, f => f.Company.CompanyName())
            .RuleFor(r => r.BankType, f => f.Random.Int(1, 10))
            .RuleFor(r => r.NickName, f => f.Lorem.Word())
            .FinishWith((f, r) =>
            {
                r.SetEmail(f.Person.Email);
            });
    }
}
