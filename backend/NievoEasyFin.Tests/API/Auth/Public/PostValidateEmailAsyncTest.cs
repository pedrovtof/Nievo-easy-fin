using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyFin.Application.Extensions.Enum;
using NievoEasyFin.Application.Interfaces.Enum;
using NievoEasyFin.Application.Interfaces.Request;
using NievoEasyFin.Application.Interfaces.Response;
using NievoEasyFin.Tests.Build.Request;
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyFin.Tests.API.Auth.Public;

/// <summary>
/// Tests for the POST validate:email endpoint of AuthenticatorController.
/// Validates controller delegation and response shape for all possible outcomes.
/// </summary>
public class PostValidateEmailAsyncTest : AuthenticatorTestBase
{
    public PostValidateEmailAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "Validação de email deverá ser feita com sucesso")]
    public async Task PostValidateEmailAsync_DadosValidos_RetornaSucesso()
    {
        // Arrange
        var request = new PostValidateEmailRequestBuilder();
        var okResult = BuildOk(EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_200_USER_VALIDATED.GetDescription());

        MockService.PostValidateEmailAsync(Arg.Any<PostValidateEmailRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        var result = await Controller.PostValidateEmailAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;

        responseValue.Should().NotBeNull();
        Output.WriteLine($"\n Validado sucesso na confirmação de email {request.Email} \n");
    }

    #endregion

    #region BadRequest Errors

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_400_EMPTY_EMAIL, "Email vazio" },
        new object[] { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_400_INVALID_EMAIL, "Email inválido" },
        new object[] { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_400_INVALID_TOKEN, "Token inválido" },
        new object[] { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_404_USER_BLOCKED_OR_VALIDATED, "Usuário já validado ou bloqueado" },
        new object[] { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_404_WRONG_TOKEN, "Token não confere" },
        new object[] { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_200_ERROR_VALIDATE_EMAIL, "Erro ao validar email" },
    };

    [Theory(DisplayName = "Validação de email deverá retornar BadRequest para cenários de erro")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task PostValidateEmailAsync_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var request = new PostValidateEmailRequestBuilder();
        var badRequestResult = BuildBadRequest(enumError);

        MockService.PostValidateEmailAsync(Arg.Any<PostValidateEmailRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        // Act
        var result = await Controller.PostValidateEmailAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region NotFound Errors

    public static IEnumerable<object[]> NotFoundErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_404_USER_NOT_FOUND, "Usuário não encontrado" },
        new object[] { EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_404_TOKEN_NOTFOUND_IN_CACHE, "Token não encontrado no cache" },
    };

    [Theory(DisplayName = "Validação de email deverá retornar NotFound para cenários de erro")]
    [MemberData(nameof(NotFoundErrors))]
    public async Task PostValidateEmailAsync_CenarioDeErro_RetornaNotFound(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var request = new PostValidateEmailRequestBuilder();
        var notFoundResult = BuildNotFound(enumError);

        MockService.PostValidateEmailAsync(Arg.Any<PostValidateEmailRequest>())
                   .Returns(Task.FromResult<IActionResult>(notFoundResult));

        // Act
        var result = await Controller.PostValidateEmailAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region Service Delegation

    [Fact(DisplayName = "Validação de email deve delegar a chamada ao service exatamente uma vez")]
    public async Task PostValidateEmailAsync_QuandoChamado_DeveDelegarAoService()
    {
        // Arrange
        var request = new PostValidateEmailRequestBuilder();
        var okResult = BuildOk(EnumErrosApi.POSTVALIDATEEMAILASYNC_AUTHSERVICE_200_USER_VALIDATED.GetDescription());

        MockService.PostValidateEmailAsync(Arg.Any<PostValidateEmailRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        await Controller.PostValidateEmailAsync(request);

        // Assert — verifica que o service foi chamado exatamente 1 vez
        await MockService.Received(1).PostValidateEmailAsync(Arg.Any<PostValidateEmailRequest>());
        Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
    }

    #endregion
}
