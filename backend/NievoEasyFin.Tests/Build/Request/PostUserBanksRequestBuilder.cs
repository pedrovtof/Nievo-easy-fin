using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostUserBanksRequest.
/// Default values are set in the constructor — no need to call Default().
/// </summary>
public class PostUserBanksRequestBuilder : PostUserBanksRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostUserBanksRequestBuilder()
    {
        SetEmail(_faker.Person.Email);
        NickName = _faker.Finance.AccountName();
        BankType = _faker.Random.Int(1, 100);
        BankName = _faker.Company.CompanyName();
    }

    public PostUserBanksRequestBuilder WithEmail(string email)
    {
        SetEmail(email);
        return this;
    }

    public PostUserBanksRequestBuilder WithNickName(string nickname)
    {
        NickName = nickname;
        return this;
    }

    public PostUserBanksRequestBuilder WithBankType(int bankType)
    {
        BankType = bankType;
        return this;
    }

    public PostUserBanksRequestBuilder WithBankName(string bankName)
    {
        BankName = bankName;
        return this;
    }
}
