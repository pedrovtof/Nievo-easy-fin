using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NSubstitute;
using Xunit.Abstractions;
using NievoEasyFin.Tests.Build.Request;

namespace NievoEasyFin.Tests.API.Core.Public;

/// <summary>
/// Tests for the GET user:bank-card endpoint of AccountsController.
/// One test per possible output of the endpoint.
/// </summary>
public class GetUserCardAsyncTest : AccountsTestBase
{
    public GetUserCardAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "Busca de cartões do usuário deverá retornar sucesso com os dados")]
    public async Task GetUserCardAsync_DadosValidos_RetornaSucesso()
    {
        // Arrange
        var okResult = BuildOk(new List<GetUserBankCardResponse>());

        MockService.GetUserCard(Arg.Any<GetUserCardRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        var fakeToken = "fake-token";

        // Act
        var request = new GetUserCardRequestBuilder();
        var result = await Controller.GetUserCard(fakeToken, request);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.Value.Should().BeOfType<ResponseApiSucess>();
        Output.WriteLine("\n Validado sucesso na busca de cartões do usuário \n");
    }

    #endregion

    #region BadRequest Errors

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.GETUSERCARDASYNC_CORESERVICE_400_INVALID_PAGE_SIZE, "Page size inválido" },
        new object[] { EnumErrosApi.GETUSERCARDASYNC_CORESERVICE_400_INVALID_PAGE, "Page inválido" },
        new object[] { EnumErrosApi.GETUSERCARDASYNC_CORESERVICE_400_INVALID_BANK_ID, "Bank id inválido" },
        new object[] { EnumErrosApi.GETUSERCARDASYNC_CORESERVICE_404_USER_NOT_FOUND, "Usuário não encontrado" },
    };

    [Theory(DisplayName = "Busca de cartões do usuário deverá retornar BadRequest para cenários de erro")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task GetUserCardAsync_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var badRequestResult = BuildBadRequest(enumError);

        MockService.GetUserCard(Arg.Any<GetUserCardRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var fakeToken = "fake-token";

        // Act
        var request = new GetUserCardRequestBuilder();
        var result = await Controller.GetUserCard(fakeToken, request);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region Service Delegation

    [Fact(DisplayName = "Busca de cartões do usuário deve delegar a chamada ao service exatamente uma vez")]
    public async Task GetUserCardAsync_QuandoChamado_DeveDelegarAoService()
    {
        // Arrange
        var okResult = BuildOk(new { });

        MockService.GetUserCard(Arg.Any<GetUserCardRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        var fakeToken = "fake-token";

        // Act
        var request = new GetUserCardRequestBuilder();
        await Controller.GetUserCard(fakeToken, request);

        // Assert
        await MockService.Received(1).GetUserCard(Arg.Any<GetUserCardRequest>());
        Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
    }

    #endregion
}
