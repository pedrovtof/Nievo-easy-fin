using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostUserCardRequest.
/// </summary>
public class PostUserCardRequestBuilder : PostUserCardRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostUserCardRequestBuilder()
    {
        SetEmail(_faker.Person.Email);
        BankId = _faker.Random.Int(1, 100);
        CardUserName = _faker.Finance.AccountName();
        CardId = _faker.Random.Int(1, 100);
        ExpireAt = _faker.Date.Future();
    }

    public PostUserCardRequestBuilder WithEmail(string email)
    {
        SetEmail(email);
        return this;
    }

    public PostUserCardRequestBuilder WithBankId(int bankId)
    {
        BankId = bankId;
        return this;
    }

    public PostUserCardRequestBuilder WithCardUserName(string cardUserName)
    {
        CardUserName = cardUserName;
        return this;
    }

    public PostUserCardRequestBuilder WithCardId(int cardId)
    {
        CardId = cardId;
        return this;
    }

    public PostUserCardRequestBuilder WithExpireAt(DateTime expireAt)
    {
        ExpireAt = expireAt;
        return this;
    }
}
