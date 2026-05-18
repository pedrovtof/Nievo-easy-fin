using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class PostCreateUserRequestFaker
{
    public static Faker<PostCreateUserRequest> Create()
    {
        return new Faker<PostCreateUserRequest>()
            .RuleFor(r => r.Name, f => f.Person.FullName)
            .RuleFor(r => r.Email, f => f.Person.Email)
            .RuleFor(r => r.Password, f => f.Internet.Password(8));
    }
}
