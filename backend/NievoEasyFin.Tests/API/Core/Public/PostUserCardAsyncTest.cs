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
/// Tests for the POST user:bank-card endpoint of AccountsController.
/// </summary>
public class PostUserCardAsyncTest : AccountsTestBase
{
    public PostUserCardAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "Criação de cartão do usuário deverá retornar sucesso com os dados")]
    public async Task PostUserCardAsync_DadosValidos_RetornaSucesso()
    {
        // Arrange
        var okResult = BuildOk(new { });

        MockService.PostUserCard(Arg.Any<PostUserCardRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        var fakeToken = "fake-token";

        // Act
        var request = new PostUserCardRequestBuilder();
        var result = await Controller.PostUserCard(fakeToken, request);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.Value.Should().BeOfType<ResponseApiSucess>();
        Output.WriteLine("\n Validado sucesso na criação do cartão do usuário \n");
    }

    #endregion

    #region BadRequest Errors

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_INVALID_EMPTY_EMAIL, "Email vazio" },
        new object[] { EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_INVALID_EMAIL, "Email inválido" },
        new object[] { EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_INVALID_BANK, "Banco inválido" },
        new object[] { EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_INVALID_CARD, "Cartão inválido" },
        new object[] { EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_INVALID_CARDNAME, "Nome no cartão inválido" },
        new object[] { EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_INVALID_EXPIREDAT, "Data de expiração inválida" },
        new object[] { EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_404_BANK_NOT_FOUND, "Banco não encontrado" },
        new object[] { EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_404_BANKCARD_NOT_FOUND, "Cartão do banco não encontrado" },
        new object[] { EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_404_USER_NOT_FOUND, "Usuário não encontrado" },
        new object[] { EnumErrosApi.POSTUSERCARDASYNC_CORESERVICE_400_CARD_NOT_CREATED, "Cartão não pôde ser criado" },
    };

    [Theory(DisplayName = "Criação de cartão do usuário deverá retornar BadRequest para cenários de erro")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task PostUserCardAsync_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var badRequestResult = BuildBadRequest(enumError);

        MockService.PostUserCard(Arg.Any<PostUserCardRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        var fakeToken = "fake-token";

        // Act
        var request = new PostUserCardRequestBuilder();
        var result = await Controller.PostUserCard(fakeToken, request);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region Service Delegation

    [Fact(DisplayName = "Criação de cartão do usuário deve delegar a chamada ao service exatamente uma vez")]
    public async Task PostUserCardAsync_QuandoChamado_DeveDelegarAoService()
    {
        // Arrange
        var okResult = BuildOk(new { });

        MockService.PostUserCard(Arg.Any<PostUserCardRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        var fakeToken = "fake-token";

        // Act
        var request = new PostUserCardRequestBuilder();
        await Controller.PostUserCard(fakeToken, request);

        // Assert
        await MockService.Received(1).PostUserCard(Arg.Any<PostUserCardRequest>());
        Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
    }

    #endregion
}
