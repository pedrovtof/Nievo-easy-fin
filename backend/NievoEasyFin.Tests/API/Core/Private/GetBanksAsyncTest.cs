using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using NievoEasyFin.Tests.Mocks.Fakers;
using Xunit;
using Xunit.Abstractions;
using NievoEasyFin.Tests.Build.Request;

using NievoEasyFin.Tests.API.Core.Public;

namespace NievoEasyFin.Tests.API.Core.Private;

public class GetBanksAsyncTest : AccountsServiceTestBase
{
    public GetBanksAsyncTest(ITestOutputHelper output) : base(output) { }

    #region BadRequest Errors

    [Fact(DisplayName = "GetBanks With Invalid Page Returns BadRequest")]
    public async Task GetBanks_WithInvalidPage_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetBanksRequestBuilder();
        request.Page = 0;
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().NotBeEmpty();
        Output.WriteLine("Validation executed successfully.");
    }

    [Fact(DisplayName = "GetBanks With Invalid Page Size Returns BadRequest")]
    public async Task GetBanks_WithInvalidPageSize_ReturnsBadRequest()
    {
        // Arrange
        var request = new GetBanksRequestBuilder();
        request.PageSize = 51;
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBanks(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = (ResponseApiError)badRequest.Value!;
        response.Messages.Should().NotBeEmpty();
        Output.WriteLine("Validation executed successfully.");
    }

    #endregion

    #region Success

    [Fact(DisplayName = "GetBanks When No Banks Exist Returns Ok With Empty List")]
    public async Task GetBanks_WhenNoBanksExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        var request = new GetBanksRequestBuilder();
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();
        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBanks(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeOfType<ResponsePaginationBase<GetBanksResponse>>();

        var pagination = (ResponsePaginationBase<GetBanksResponse>)response.Data;
        pagination.Items.Should().BeEmpty();
        pagination.Records.Should().Be(0);
        Output.WriteLine("Success test executed correctly.");
    }

    [Fact(DisplayName = "GetBanks When Banks Exist Returns Ok With Banks List")]
    public async Task GetBanks_WhenBanksExist_ReturnsOkWithBanksList()
    {
        // Arrange
        var request = new GetBanksRequestBuilder();
        request.Page = 1;
        var (origin, replica) = CreateSharedCoreContexts();
        var (authOrigin, authReplica) = DbContextMockFactory.CreateSharedAuthContexts();

        // Seed data using SQL because Dapper queries accounts.bank but EF Core inserts into main.bank
        using (var cmd = origin.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandText = @"
                INSERT INTO accounts.bank_type (id, name, description, active, created_at, updated_at) 
                VALUES (1, 'Conta Corrente', 'Desc', 1, '2023-01-01', '2023-01-01');

                INSERT INTO accounts.bank (id, name, bank_type, active, created_at, updated_at) 
                VALUES (1, 'Bank 1', 1, 1, '2023-01-01', '2023-01-01'),
                       (2, 'Bank 2', 1, 1, '2023-01-01', '2023-01-01');
            ";
            cmd.ExecuteNonQuery();
        }

        var service = CreateService(origin, replica, authOrigin, authReplica);

        // Act
        var result = await service.GetBanks(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ResponseApiSucess>();

        var response = (ResponseApiSucess)okResult.Value!;
        response.Data.Should().BeOfType<ResponsePaginationBase<GetBanksResponse>>();

        var pagination = (ResponsePaginationBase<GetBanksResponse>)response.Data;
        pagination.Items.Should().NotBeEmpty();
        pagination.Records.Should().BeGreaterThan(0);
        Output.WriteLine("Success test executed correctly.");
    }

    #endregion
}
