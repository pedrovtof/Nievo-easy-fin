using Bogus;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Tests.Build.Generators;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostLoginUserRequest.
/// Default values are set in the constructor — no need to call Default().
/// </summary>
public class PostLoginUserRequestBuilder : PostLoginUserRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostLoginUserRequestBuilder()
    {
        Email = _faker.Person.Email;
        Password = PasswordGenerator.Generate();
    }

    public PostLoginUserRequestBuilder WithEmail(string email)
    {
        Email = email;
        return this;
    }

    public PostLoginUserRequestBuilder WithPassword(string password)
    {
        Password = password;
        return this;
    }
}
