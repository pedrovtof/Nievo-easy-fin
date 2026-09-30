using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostUserCategoryRequest.
/// </summary>
public class PostUserCategoryRequestBuilder : PostUserCategoryRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostUserCategoryRequestBuilder()
    {
        SetEmail(_faker.Person.Email);
        Name = _faker.Commerce.Categories(1)[0];
        Description = _faker.Commerce.ProductDescription();
        Goal = _faker.Random.Int(1, 100);
        ParentCategory = null;
    }

    public PostUserCategoryRequestBuilder WithEmail(string email)
    {
        SetEmail(email);
        return this;
    }

    public PostUserCategoryRequestBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public PostUserCategoryRequestBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public PostUserCategoryRequestBuilder WithGoal(int? goal)
    {
        Goal = goal;
        return this;
    }

    public PostUserCategoryRequestBuilder WithParentCategory(int? parentCategory)
    {
        ParentCategory = parentCategory;
        return this;
    }
}
