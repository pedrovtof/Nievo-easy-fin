using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostAccountsBanksRequest.
/// </summary>
public class PostAccountsBanksRequestBuilder : PostAccountsBanksRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostAccountsBanksRequestBuilder()
    {
        Name = _faker.Company.CompanyName();
        BankType = _faker.Random.Int(1, 10);
    }
}
