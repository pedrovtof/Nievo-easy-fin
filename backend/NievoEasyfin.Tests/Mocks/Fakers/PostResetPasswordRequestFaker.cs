using Bogus;
using NievoEasyfin.Application.Interfaces.Request;

namespace NievoEasyfin.Tests.Mocks.Fakers;

public static class PostResetPasswordRequestFaker
{
    public static Faker<PostResetPasswordRequest> Create()
    {
        return new Faker<PostResetPasswordRequest>()
            .RuleFor(r => r.Email, f => f.Person.Email);
    }
}
