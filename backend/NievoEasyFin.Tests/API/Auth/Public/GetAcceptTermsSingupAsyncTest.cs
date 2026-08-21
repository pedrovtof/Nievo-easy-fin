using NievoEasyFin.Tests.Mocks.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NievoEasyFin.Application.Data.Views;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Mocks.Database;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Auth.Public;

public class GetAcceptTermsSingupAsyncTest : AuthenticatorServiceTestBase
{
    public GetAcceptTermsSingupAsyncTest(WireMockFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    #region Success
    [Fact(DisplayName = "GetAcceptTermsSingupAsync: When terms exist, returns Ok with AcceptTermsViews")]
    public async Task GetAcceptTermsSingupAsync_WhenTermsExist_ReturnsOkWithAcceptTermsViews()
    {
        // Arrange
        Output.WriteLine("Arranging GetAcceptTermsSingup test for success.");
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();

        var codeSingupTerms = Environment.GetEnvironmentVariable("CODE_SINGUP_TERMS") ?? "SINGUP_TERMS";

        var connection = replica.Database.GetDbConnection();
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = $@"
                INSERT INTO journey.accept_terms (code, name, description, version, content, created_at, updated_at, active)
                VALUES ('{codeSingupTerms}', 'Termos de Uso', 'Termos de uso do sistema', 1, 'Conteúdo dos termos versão [VERSION] atualizado em [ENTITY_UPDATED_AT_COLUMN]', datetime('now'), datetime('now'), 1);
            ";
            cmd.ExecuteNonQuery();
        }

        var service = CreateService(origin, replica);

        // Act
        Output.WriteLine("Executing GetAcceptTermsSingupAsync.");
        var result = await service.GetAcceptTermsSingupAsync();

        // Assert
        Output.WriteLine("Validating result.");
        if (result is BadRequestObjectResult badReq)
        {
            var err = (ResponseApiError)badReq.Value!;
            throw new Exception($"GetAcceptTermsSingupAsync failed with: {string.Join(", ", err.Messages)}");
        }

        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        var response = okResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;
        response.Data.Should().BeOfType<AcceptTermsViews>();
    }
    #endregion

    #region BadRequest Errors
    [Fact(DisplayName = "GetAcceptTermsSingupAsync: When terms not found, returns BadRequest")]
    public async Task GetAcceptTermsSingupAsync_WhenTermsNotFound_ReturnsBadRequest()
    {
        // Arrange
        Output.WriteLine("Arranging GetAcceptTermsSingup test for terms not found.");
        var (origin, replica) = DbContextMockFactory.CreateSharedAuthContexts();

        var service = CreateService(origin, replica);

        // Act
        Output.WriteLine("Executing GetAcceptTermsSingupAsync.");
        var result = await service.GetAcceptTermsSingupAsync();

        // Assert
        Output.WriteLine("Validating result.");
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = (BadRequestObjectResult)result;
        var response = badRequest.Value.Should().BeOfType<ResponseApiError>().Subject;
        response.Messages.Should().Contain(
            EnumErrosApi.GETACCEPTTERMSASYNC_AUTHSERVICE_400_TERMS_NOT_FOUND.GetDescription()
        );
    }
    #endregion
}
