using Bogus;
using NievoEasyfin.Application.Interfaces.Request;

namespace NievoEasyfin.Tests.Mocks.Fakers;

public static class PatchResetPasswordRequestFaker
{
    public static Faker<PatchResetPasswordRequest> Create()
    {
        return new Faker<PatchResetPasswordRequest>()
            .RuleFor(r => r.Email, f => f.Person.Email)
            .RuleFor(r => r.PinToken, f => f.Random.Number(100000, 999999).ToString())
            .RuleFor(r => r.Password, f => f.Internet.Password(8));
    }
}
