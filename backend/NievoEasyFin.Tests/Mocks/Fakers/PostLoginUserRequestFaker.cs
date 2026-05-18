using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Mocks.Fakers;

public static class PostLoginUserRequestFaker
{
    public static Faker<PostLoginUserRequest> Create()
    {
        return new Faker<PostLoginUserRequest>()
            .RuleFor(r => r.Email, f => f.Person.Email)
            .RuleFor(r => r.Password, f => f.Internet.Password(8));
    }
}
