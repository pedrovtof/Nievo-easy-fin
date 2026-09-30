using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

public class GetUserGoalRequestBuilder : GetUserGoalRequest
{
    public GetUserGoalRequestBuilder()
    {
        Page = 1;
        PageSize = 10;
        Active = true;
        SetEmail(new Faker().Internet.Email());
    }

    public GetUserGoalRequestBuilder WithEmail(string email)
    {
        SetEmail(email);
        return this;
    }

    public GetUserGoalRequestBuilder WithInvalidPage()
    {
        Page = 0;
        return this;
    }

    public GetUserGoalRequestBuilder WithInvalidEmail()
    {
        SetEmail("invalid-email");
        return this;
    }

    public GetUserGoalRequestBuilder WithActive(bool active)
    {
        Active = active;
        return this;
    }
}
