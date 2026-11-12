using Bogus;
using NievoEasyFin.Application.Interfaces.Request;

namespace NievoEasyFin.Tests.Build.Request;

/// <summary>
/// Fluent builder for GetPaymentTransactionRequest.
/// </summary>
public class GetPaymentTransactionRequestBuilder : GetPaymentTransactionRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public GetPaymentTransactionRequestBuilder()
    {
        Page = 1;
        PageSize = 10;
        SetEmail(_faker.Person.Email);
    }

    public GetPaymentTransactionRequestBuilder WithEmail(string email)
    {
        SetEmail(email);
        return this;
    }

    public GetPaymentTransactionRequestBuilder WithPage(int page)
    {
        Page = page;
        return this;
    }

    public GetPaymentTransactionRequestBuilder WithPageSize(int pageSize)
    {
        PageSize = pageSize;
        return this;
    }

    public GetPaymentTransactionRequestBuilder WithInvalidPage()
    {
        Page = 0;
        return this;
    }

    public GetPaymentTransactionRequestBuilder WithInvalidPageSize()
    {
        PageSize = 0;
        return this;
    }

    public GetPaymentTransactionRequestBuilder WithTransactionSituationId(int? situationId)
    {
        TransactionSituationId = situationId;
        return this;
    }
}
