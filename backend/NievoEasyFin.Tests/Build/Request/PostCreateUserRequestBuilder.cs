using Bogus;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Tests.Build.Generators;

namespace NievoEasyFin.Tests.Build.Request;

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
        AcceptTerms = true;
        SetHost("localhost");
        SetUserAgent("TestAgent/1.0");
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

    public PostCreateUserRequestBuilder WithAcceptTerms(bool acceptTerms)
    {
        AcceptTerms = acceptTerms;
        return this;
    }

    public PostCreateUserRequestBuilder WithHost(string host)
    {
        SetHost(host);
        return this;
    }

    public PostCreateUserRequestBuilder WithUserAgent(string userAgent)
    {
        SetUserAgent(userAgent);
        return this;
    }
}
