using Bogus;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Tests.Build.Generators;

namespace NievoEasyfin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostCreateUserRequest.
/// </summary>
public class PostCreateUserRequestBuilder : PostCreateUserRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostCreateUserRequestBuilder()
    {
        Name = _faker.Person.FullName;
        Email = _faker.Person.Email;
        Password = PasswordGenerator.Generate();
    }

    public PostCreateUserRequestBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public PostCreateUserRequestBuilder WithEmail(string email)
    {
        Email = email;
        return this;
    }

    public PostCreateUserRequestBuilder WithPassword(string password)
    {
        Password = password;
        return this;
    }
}
