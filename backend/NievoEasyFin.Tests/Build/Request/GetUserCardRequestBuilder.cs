using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for GetUserCardRequest.
/// </summary>
public class GetUserCardRequestBuilder : GetUserCardRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public GetUserCardRequestBuilder()
    {
        SetEmail(_faker.Person.Email);
        BankId = _faker.Random.Int(1, 100);
        Active = true;
        Page = 1;
        PageSize = 10;
    }

    public GetUserCardRequestBuilder WithEmail(string email)
    {
        SetEmail(email);
        return this;
    }

    public GetUserCardRequestBuilder WithBankId(int bankId)
    {
        BankId = bankId;
        return this;
    }

    public GetUserCardRequestBuilder WithActive(bool active)
    {
        Active = active;
        return this;
    }
}
