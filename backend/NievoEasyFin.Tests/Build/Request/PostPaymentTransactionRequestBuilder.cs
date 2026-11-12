using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for PostPaymentTransactionRequest.
/// </summary>
public class PostPaymentTransactionRequestBuilder : PostPaymentTransactionRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PostPaymentTransactionRequestBuilder()
    {
        SetEmail(_faker.Person.Email);
        TransactionStatus = _faker.Random.Int(2, 10);
        TransactionSituation = _faker.Random.Int(1, 10);
        CategoryName = _faker.Commerce.Categories(1)[0];
        PayDate = _faker.Date.Recent();
        Amount = _faker.Random.Long(100, 100000);
        Card = _faker.Random.Int(1, 10);
        Bank = _faker.Random.Int(1, 10);
        TransactionType = _faker.Random.Int(2, 10);
        Scheduler = _faker.Random.Int(1, 10);
        Name = _faker.Commerce.ProductName();
        Description = _faker.Commerce.ProductDescription();
    }

    public PostPaymentTransactionRequestBuilder WithEmail(string email)
    {
        SetEmail(email);
        return this;
    }

    public PostPaymentTransactionRequestBuilder WithTransactionStatus(int transactionStatus)
    {
        TransactionStatus = transactionStatus;
        return this;
    }

    public PostPaymentTransactionRequestBuilder WithCategoryName(string categoryName)
    {
        CategoryName = categoryName;
        return this;
    }

    public PostPaymentTransactionRequestBuilder WithPayDate(DateTime payDate)
    {
        PayDate = payDate;
        return this;
    }

    public PostPaymentTransactionRequestBuilder WithAmount(long amount)
    {
        Amount = amount;
        return this;
    }

    public PostPaymentTransactionRequestBuilder WithCard(int card)
    {
        Card = card;
        return this;
    }

    public PostPaymentTransactionRequestBuilder WithBank(int bank)
    {
        Bank = bank;
        return this;
    }

    public PostPaymentTransactionRequestBuilder WithTransactionType(int transactionType)
    {
        TransactionType = transactionType;
        return this;
    }

    public PostPaymentTransactionRequestBuilder WithScheduler(int scheduler)
    {
        Scheduler = scheduler;
        return this;
    }

    public PostPaymentTransactionRequestBuilder WithName(string name)
    {
        Name = name;
        return this;
    }

    public PostPaymentTransactionRequestBuilder WithDescription(string description)
    {
        Description = description;
        return this;
    }

    public PostPaymentTransactionRequestBuilder WithTransactionSituation(int transactionSituation)
    {
        TransactionSituation = transactionSituation;
        return this;
    }
}
