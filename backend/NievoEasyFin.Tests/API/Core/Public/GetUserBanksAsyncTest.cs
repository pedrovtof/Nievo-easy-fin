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
/// Tests for the GET user-banks endpoint of AccountsController.
/// One test per possible output of the endpoint.
/// </summary>
public class GetUserBanksAsyncTest : AccountsTestBase
{
    public GetUserBanksAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "Busca de contas bancárias do usuário deverá retornar sucesso com os dados")]
    public async Task GetUserBanksAsync_DadosValidos_RetornaSucesso()
    {
        // Arrange
        var okResult = BuildOk(new List<GetUserBanksResponse>());

        MockService.GetUserBanks(Arg.Any<GetUserBanksRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        var fakeToken = "fake-token"; // GetClaimValue will return null for this, but that's fine for the controller logic test.

        // Act
        var request = new GetUserBanksRequestBuilder();
        var result = await Controller.GetUserBanks(fakeToken, request);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.Value.Should().BeOfType<ResponseApiSucess>();
        Output.WriteLine("\n Validado sucesso na busca das contas bancárias do usuário \n");
    }

    #endregion

    #region BadRequest Errors

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.GETUSERBANKSASYNC_CORESERVICE_400_EMPTY_EMAIL, "Email vazio" },
        new object[] { EnumErrosApi.GETUSERBANKSASYNC_CORESERVICE_400_INVALID_EMAIL, "Email inválido" },
        new object[] { EnumErrosApi.GETUSERBANKSASYNC_CORESERVICE_404_USER_NOT_FOUND, "Usuário não encontrado" },
    };

    [Theory(DisplayName = "Busca de contas bancárias do usuário deverá retornar BadRequest para cenários de erro")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task GetUserBanksAsync_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var badRequestResult = BuildBadRequest(enumError);

        MockService.GetUserBanks(Arg.Any<GetUserBanksRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var fakeToken = "fake-token";

        // Act
        var request = new GetUserBanksRequestBuilder();
        var result = await Controller.GetUserBanks(fakeToken, request);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region Service Delegation

    [Fact(DisplayName = "Busca de contas bancárias do usuário deve delegar a chamada ao service exatamente uma vez")]
    public async Task GetUserBanksAsync_QuandoChamado_DeveDelegarAoService()
    {
        // Arrange
        var okResult = BuildOk(new { });

        MockService.GetUserBanks(Arg.Any<GetUserBanksRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        var fakeToken = "fake-token";

        // Act
        var request = new GetUserBanksRequestBuilder();
        await Controller.GetUserBanks(fakeToken, request);

        // Assert
        await MockService.Received(1).GetUserBanks(Arg.Any<GetUserBanksRequest>());
        Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
    }

    #endregion
}
