using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Core.Public;

public class PostPaymentTransactionAsyncTest : AccountsServiceTestBase
{
    public PostPaymentTransactionAsyncTest(ITestOutputHelper output) : base(output) { }

    #region BadRequest Errors

    [Fact(DisplayName = "PostPaymentTransaction When Email Is Empty Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenEmailIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithEmail("");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("must not be empty."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Email Is Invalid Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenEmailIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithEmail("invalid-email");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("is not a valid email address."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Card Is Invalid Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenCardIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithCard(-1);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Card' must be greater than or equal to '0'."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Bank Is Invalid Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenBankIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithBank(-1);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Bank' must be greater than or equal to '0'."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When TransactionStatus Is Invalid Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenTransactionStatusIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithTransactionStatus(0);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Transaction Status' must be greater than '0'."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Amount Is Invalid Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenAmountIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithAmount(0);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Amount' must be greater than '0'."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When TransactionType Is Invalid Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenTransactionTypeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithTransactionType(0);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Transaction Type' must be greater than '0'."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Scheduler Is Invalid Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenSchedulerIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithScheduler(-1);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Scheduler' must be greater than or equal to '0'."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Description Is Null Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenDescriptionIsNull_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithDescription(null!);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Description' must not be empty."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Description Is Empty Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenDescriptionIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithDescription("");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Description' must not be empty."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Name Is Null Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenNameIsNull_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithName(null!);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Name' must not be empty."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Name Is Empty Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenNameIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithName("");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Name' must not be empty."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When CategoryName Is Null Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenCategoryNameIsNull_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithCategoryName(null!);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Category Name' must not be empty."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When CategoryName Is Empty Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenCategoryNameIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithCategoryName("");
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Category Name' must not be empty."));
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When PayDate Is Invalid Returns BadRequest")]
    public async Task PostPaymentTransaction_WhenPayDateIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithPayDate(new DateTime(1899, 12, 31));
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Pay Date' must be greater than"));
        Output.WriteLine("Validation executed successfully.");
    }

    #endregion

    #region NotFound Errors

    [Fact(DisplayName = "PostPaymentTransaction When User Not Found Returns NotFound")]
    public async Task PostPaymentTransaction_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_USER_NOT_FOUND.GetDescription());
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Bank Not Found Returns NotFound")]
    public async Task PostPaymentTransaction_WhenBankNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_BANK_NOT_FOUND.GetDescription());
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Card Not Found Returns NotFound")]
    public async Task PostPaymentTransaction_WhenCardNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithCard(999);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var userBank = UserBankEntityFaker.Create().Generate();
        userBank.UserId = existingUser.Id;
        userBank.BankId = request.Bank;
        origin.UserBank.Add(userBank);
        await origin.SaveChangesAsync();

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_USER_CARD_NOT_FOUND.GetDescription());
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When TransactionType Not Found Returns NotFound")]
    public async Task PostPaymentTransaction_WhenTransactionTypeNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithCard(0);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var userBank = UserBankEntityFaker.Create().Generate();
        userBank.UserId = existingUser.Id;
        userBank.BankId = request.Bank;
        origin.UserBank.Add(userBank);
        await origin.SaveChangesAsync();

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_TRANSACTION_TYPE_NOT_FOUND.GetDescription());
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When TransactionStatus Not Found Returns NotFound")]
    public async Task PostPaymentTransaction_WhenTransactionStatusNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithCard(0);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var userBank = UserBankEntityFaker.Create().Generate();
        userBank.UserId = existingUser.Id;
        userBank.BankId = request.Bank;
        origin.UserBank.Add(userBank);

        var transactionType = TransactionTypeEntityFaker.Create().Generate();
        transactionType.Id = request.TransactionType;
        origin.TransactionType.Add(transactionType);

        await origin.SaveChangesAsync();

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_TRANSACTION_STATUS_NOT_FOUND.GetDescription());
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Scheduler Not Found Returns NotFound")]
    public async Task PostPaymentTransaction_WhenSchedulerNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithCard(0).WithScheduler(999);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var userBank = UserBankEntityFaker.Create().Generate();
        userBank.UserId = existingUser.Id;
        userBank.BankId = request.Bank;
        origin.UserBank.Add(userBank);

        var transactionType = TransactionTypeEntityFaker.Create().Generate();
        transactionType.Id = request.TransactionType;
        origin.TransactionType.Add(transactionType);

        var transactionStatus = TransactionStatusEntityFaker.Create().Generate();
        transactionStatus.Id = request.TransactionStatus;
        origin.TransactionStatus.Add(transactionStatus);

        await origin.SaveChangesAsync();

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_SCHEDULER_NOT_FOUND.GetDescription());
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostPaymentTransaction When Category Not Found Returns NotFound")]
    public async Task PostPaymentTransaction_WhenCategoryNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder().WithCard(0).WithScheduler(0);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var userBank = UserBankEntityFaker.Create().Generate();
        userBank.UserId = existingUser.Id;
        userBank.BankId = request.Bank;
        origin.UserBank.Add(userBank);

        var transactionType = TransactionTypeEntityFaker.Create().Generate();
        transactionType.Id = request.TransactionType;
        origin.TransactionType.Add(transactionType);

        var transactionStatus = TransactionStatusEntityFaker.Create().Generate();
        transactionStatus.Id = request.TransactionStatus;
        origin.TransactionStatus.Add(transactionStatus);

        await origin.SaveChangesAsync();

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = (NotFoundObjectResult)result;
        var response = (ResponseApiError)notFound.Value!;
        response.Messages.Should().Contain(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_404_CATEGORY_NOT_FOUND.GetDescription());
        Output.WriteLine("Validation executed successfully.");
    }

    #endregion

    #region Success

    [Fact(DisplayName = "PostPaymentTransaction With Valid Request Including Card And Scheduler Returns Created")]
    public async Task PostPaymentTransaction_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var userBank = UserBankEntityFaker.Create().Generate();
        userBank.UserId = existingUser.Id;
        userBank.BankId = request.Bank;
        origin.UserBank.Add(userBank);

        var userBankCard = UserBankCardEntityFaker.Create().Generate();
        userBankCard.UserId = existingUser.Id;
        userBankCard.CardId = request.Card;
        userBankCard.BankId = request.Bank;
        origin.UserBankCard.Add(userBankCard);

        var transactionType = TransactionTypeEntityFaker.Create().Generate();
        transactionType.Id = request.TransactionType;
        origin.TransactionType.Add(transactionType);

        var transactionStatus = TransactionStatusEntityFaker.Create().Generate();
        transactionStatus.Id = request.TransactionStatus;
        origin.TransactionStatus.Add(transactionStatus);

        var schedulerType = SchedulerTypeEntityFaker.Create().Generate();
        schedulerType.Id = request.Scheduler;
        origin.SchedulerType.Add(schedulerType);

        var category = CategoryEntityFaker.Create().Generate();
        category.UserId = existingUser.Id;
        category.Name = request.CategoryName;
        origin.Category.Add(category);

        await origin.SaveChangesAsync();

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var createdResult = (ObjectResult)result;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)createdResult.Value!;
        response.Data.Should().Be(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_201_SUCESS.GetDescription());

        var transactionInDb = await origin.Transaction.FirstOrDefaultAsync(t => t.Name == request.Name && t.UserId == existingUser.Id);
        transactionInDb.Should().NotBeNull();
        transactionInDb!.Amount.Should().Be(request.Amount);
        transactionInDb.CardId.Should().Be(userBankCard.Id);
        transactionInDb.SchedulerId.Should().Be(schedulerType.Id);

        Output.WriteLine("Success test executed correctly.");
    }

    [Fact(DisplayName = "PostPaymentTransaction Without Card And Scheduler Returns Created")]
    public async Task PostPaymentTransaction_WithoutCardAndScheduler_ReturnsCreated()
    {
        // Arrange
        var request = new PostPaymentTransactionRequestBuilder()
            .WithCard(0)
            .WithScheduler(0);
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        var existingUser = UserEntityFaker.Create().Generate();
        existingUser.Email = request.GetEmail()!;
        authOrigin.Users.Add(existingUser);
        await authOrigin.SaveChangesAsync();

        var userBank = UserBankEntityFaker.Create().Generate();
        userBank.UserId = existingUser.Id;
        userBank.BankId = request.Bank;
        origin.UserBank.Add(userBank);

        var transactionType = TransactionTypeEntityFaker.Create().Generate();
        transactionType.Id = request.TransactionType;
        origin.TransactionType.Add(transactionType);

        var transactionStatus = TransactionStatusEntityFaker.Create().Generate();
        transactionStatus.Id = request.TransactionStatus;
        origin.TransactionStatus.Add(transactionStatus);

        var category = CategoryEntityFaker.Create().Generate();
        category.UserId = existingUser.Id;
        category.Name = request.CategoryName;
        origin.Category.Add(category);

        await origin.SaveChangesAsync();

        var service = CreatePaymentService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostPaymentTransaction(request);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var createdResult = (ObjectResult)result;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)createdResult.Value!;
        response.Data.Should().Be(EnumErrosApi.POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_201_SUCESS.GetDescription());

        var transactionInDb = await origin.Transaction.FirstOrDefaultAsync(t => t.Name == request.Name && t.UserId == existingUser.Id);
        transactionInDb.Should().NotBeNull();
        transactionInDb!.CardId.Should().BeNull();
        transactionInDb.SchedulerId.Should().BeNull();

        Output.WriteLine("Success test executed correctly.");
    }

    #endregion
}
