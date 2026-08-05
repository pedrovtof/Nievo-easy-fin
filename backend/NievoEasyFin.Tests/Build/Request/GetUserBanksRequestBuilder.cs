using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for GetUserBanksRequest.
/// Default values are set in the constructor — no need to call Default().
/// </summary>
public class GetUserBanksRequestBuilder : GetUserBanksRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public GetUserBanksRequestBuilder()
    {
        SetEmail(_faker.Person.Email);
    }

    public GetUserBanksRequestBuilder WithEmail(string email)
    {
        SetEmail(email);
        return this;
    }
}
