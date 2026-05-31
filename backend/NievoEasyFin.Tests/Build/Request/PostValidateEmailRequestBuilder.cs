using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostValidateEmailRequest.
/// Default values are set in the constructor — no need to call Default().
/// </summary>
public class PostValidateEmailRequestBuilder : PostValidateEmailRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostValidateEmailRequestBuilder()
    {
        Email = _faker.Person.Email;
        PinToken = _faker.Random.Number(100000, 999999);
    }

    public PostValidateEmailRequestBuilder WithEmail(string email)
    {
        Email = email;
        return this;
    }

    public PostValidateEmailRequestBuilder WithPinToken(int pinToken)
    {
        PinToken = pinToken;
        return this;
    }
}
