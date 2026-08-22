using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostBankCardRequest.
/// </summary>
public class PostBankCardRequestBuilder : PostBankCardRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostBankCardRequestBuilder()
    {
        Name = _faker.Commerce.ProductName();
        BankId = _faker.Random.Int(1, 100);
        CardType = _faker.Random.Int(1, 10);
        Flag = "Mastercard";
    }
}
