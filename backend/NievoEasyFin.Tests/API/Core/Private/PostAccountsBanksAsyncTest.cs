using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Entities;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit;
using Xunit.Abstractions;
using NievoEasyFin.Tests.Build.Request;

using NievoEasyFin.Tests.API.Core.Public;

namespace NievoEasyFin.Tests.API.Core.Private;

public class PostAccountsBanksAsyncTest : AccountsServiceTestBase
{
    public PostAccountsBanksAsyncTest(ITestOutputHelper output) : base(output) { }

    #region BadRequest Errors

    [Fact(DisplayName = "PostAccountsBanks With Invalid Request Returns BadRequest")]
    public async Task PostAccountsBanks_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostAccountsBanksRequestBuilder();
        request.Name = "";
        request.BankType = 0;
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostAccountsBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains("'Name' must not be empty."));
        response.Messages.Should().Contain(e => e.Contains("'Bank Type' must not be empty.") || e.Contains("'Bank Type' must be greater than or equal to '1'."));
        
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostAccountsBanks When Bank Already Exists Returns BadRequest")]
    public async Task PostAccountsBanks_WhenBankAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostAccountsBanksRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed bank
        var existingBank = BankEntityFaker.Create().Generate();
        existingBank.Name = request.Name;
        existingBank.BankType = request.BankType;
        origin.Bank.Add(existingBank);
        await origin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostAccountsBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_400_BANK_ALREADY_EXISTS.GetDescription()));
        
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "PostAccountsBanks When BankType Invalid Returns BadRequest")]
    public async Task PostAccountsBanks_WhenBankTypeInvalid_ReturnsBadRequest()
    {
        // Arrange
        var request = new PostAccountsBanksRequestBuilder();
        request.BankType = 99;
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostAccountsBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().Contain(e => e.Contains(EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_400_BANKTYPE_INVALID.GetDescription()));
        
        Output.WriteLine("Validation executed successfully.");
    }

    #endregion

    #region Success

    [Fact(DisplayName = "PostAccountsBanks With Valid Request Returns Created")]
    public async Task PostAccountsBanks_WithValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = new PostAccountsBanksRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed bank type
        var existingBankType = BankTypeEntityFaker.Create().Generate();
        existingBankType.Id = request.BankType;
        existingBankType.Name = "Conta Corrente";
        existingBankType.Description = "Conta Corrente";
        existingBankType.Active = true;
        existingBankType.CreatedAt = DateTime.UtcNow;
        origin.BankType.Add(existingBankType);
        await origin.SaveChangesAsync();

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.PostAccountsBanks(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().Be(EnumErrosApi.POSTACCOUNTSBANKS_CORESERVICE_200_CREATED.GetDescription());

        // Verify bank was created in database
        var bankInDb = await replica.Bank.FirstOrDefaultAsync(b => b.Name == request.Name && b.BankType == request.BankType);
        bankInDb.Should().NotBeNull();
        bankInDb!.Active.Should().BeTrue();
        
        Output.WriteLine("Success test executed correctly.");
    }

    #endregion
}
