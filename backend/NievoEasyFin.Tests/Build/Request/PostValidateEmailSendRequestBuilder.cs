using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostValidateEmailSendRequest.
/// Default values are set in the constructor — no need to call Default().
/// </summary>
public class PostValidateEmailSendRequestBuilder : PostValidateEmailSendRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostValidateEmailSendRequestBuilder()
    {
        Email = _faker.Person.Email;
    }

    public PostValidateEmailSendRequestBuilder WithEmail(string email)
    {
        Email = email;
        return this;
    }
}
