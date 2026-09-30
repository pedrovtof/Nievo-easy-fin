using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

public class GetUserCategoryRequestBuilder : GetUserCategoryRequest
{
    public GetUserCategoryRequestBuilder()
    {
        Page = 1;
        PageSize = 10;
        Active = true;
        SetEmail(new Faker().Internet.Email());
    }

    public GetUserCategoryRequestBuilder WithEmail(string email)
    {
        SetEmail(email);
        return this;
    }

    public GetUserCategoryRequestBuilder WithInvalidPage()
    {
        Page = 0;
        return this;
    }

    public GetUserCategoryRequestBuilder WithInvalidEmail()
    {
        SetEmail("invalid-email");
        return this;
    }

    public GetUserCategoryRequestBuilder WithActive(bool active)
    {
        Active = active;
        return this;
    }
}
