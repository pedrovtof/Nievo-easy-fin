using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Response;
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Auth.Public;

/// <summary>
/// Tests for the GET accept-terms:singup endpoint of AuthenticatorController.
/// One test per possible output of the endpoint.
/// </summary>
public class GetAcceptTermsSingupAsyncTest : AuthenticatorTestBase
{
    public GetAcceptTermsSingupAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "Busca dos termos de aceite deverá retornar sucesso com os dados")]
    public async Task GetAcceptTermsSingupAsync_DadosValidos_RetornaSucesso()
    {
        // Arrange
        var okResult = BuildOk(new { Content = "Termos de uso...", Version = 1 });

        MockService.GetAcceptTermsSingupAsync()
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        var result = await Controller.GetAcceptTermsSingupAsync();

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.Value.Should().BeOfType<ResponseApiSucess>();
        Output.WriteLine("\n Validado sucesso na busca dos termos de aceite \n");
    }

    #endregion

    #region BadRequest Errors

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.GETACCEPTTERMSASYNC_AUTHSERVICE_400_TERMS_NOT_FOUND, "Termos não encontrados" },
    };

    [Theory(DisplayName = "Busca dos termos deverá retornar BadRequest para cenários de erro")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task GetAcceptTermsSingupAsync_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var badRequestResult = BuildBadRequest(enumError);

        MockService.GetAcceptTermsSingupAsync()
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        // Act
        var result = await Controller.GetAcceptTermsSingupAsync();

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region Service Delegation

    [Fact(DisplayName = "Busca dos termos deve delegar a chamada ao service exatamente uma vez")]
    public async Task GetAcceptTermsSingupAsync_QuandoChamado_DeveDelegarAoService()
    {
        // Arrange
        var okResult = BuildOk(new { Content = "Termos de uso...", Version = 1 });

        MockService.GetAcceptTermsSingupAsync()
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        await Controller.GetAcceptTermsSingupAsync();

        // Assert
        await MockService.Received(1).GetAcceptTermsSingupAsync();
        Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
    }

    #endregion
}
