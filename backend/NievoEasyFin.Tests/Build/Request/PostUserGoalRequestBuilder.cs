using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostUserGoalRequest.
/// </summary>
public class PostUserGoalRequestBuilder : PostUserGoalRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostUserGoalRequestBuilder()
    {
        SetEmail(_faker.Person.Email);
        Name = _faker.Commerce.ProductName();
        Description = _faker.Commerce.ProductDescription();
        Amount = _faker.Random.Int(100, 10000);
        IsPercent = false;
        ExpireAt = _faker.Date.Future();
    }

    public PostUserGoalRequestBuilder WithEmail(string email)
    {
        SetEmail(email);
        return this;
    }

    public PostUserGoalRequestBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public PostUserGoalRequestBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public PostUserGoalRequestBuilder WithAmount(int amount)
    {
        Amount = amount;
        return this;
    }

    public PostUserGoalRequestBuilder WithIsPercent(bool isPercent)
    {
        IsPercent = isPercent;
        return this;
    }

    public PostUserGoalRequestBuilder WithExpireAt(DateTime expireAt)
    {
        ExpireAt = expireAt;
        return this;
    }
}
