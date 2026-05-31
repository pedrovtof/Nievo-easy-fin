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
/// Tests for the POST send-validate:email endpoint of AuthenticatorController.
/// One test per possible output of the endpoint.
/// </summary>
public class PostValidateEmailSendAsyncTest : AuthenticatorTestBase
{
    public PostValidateEmailSendAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "Reenvio do token de validação deverá ser feito com sucesso")]
    public async Task PostValidateEmailSendAsync_DadosValidos_RetornaSucesso()
    {
        // Arrange
        var request = new PostValidateEmailSendRequestBuilder();
        var okResult = BuildOk(EnumErrosApi.POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_200_TOKEN_CREATED.GetDescription());

        MockService.PostValidateEmailSendAsync(Arg.Any<PostValidateEmailSendRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        var result = await Controller.PostValidateEmailSendAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.Value.Should().BeOfType<ResponseApiSucess>();
        Output.WriteLine($"\n Validado sucesso no reenvio do token para {request.Email} \n");
    }

    #endregion

    #region BadRequest Errors

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_400_EMPTY_EMAIL,              "Email vazio" },
        new object[] { EnumErrosApi.POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_400_INVALID_EMAIL,            "Email inválido" },
        new object[] { EnumErrosApi.POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_404_USER_BLOCKED_OR_VALIDATED, "Usuário já validado ou bloqueado" },
        new object[] { EnumErrosApi.POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_400_TOKEN_FOUND_IN_CACHE,     "Token já existe no cache" },
    };

    [Theory(DisplayName = "Reenvio do token deverá retornar BadRequest para cenários de erro")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task PostValidateEmailSendAsync_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var request = new PostValidateEmailSendRequestBuilder();
        var badRequestResult = BuildBadRequest(enumError);

        MockService.PostValidateEmailSendAsync(Arg.Any<PostValidateEmailSendRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        // Act
        var result = await Controller.PostValidateEmailSendAsync(request);

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
        new object[] { EnumErrosApi.POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_404_USER_NOT_FOUND, "Usuário não encontrado" },
    };

    [Theory(DisplayName = "Reenvio do token deverá retornar NotFound para cenários de erro")]
    [MemberData(nameof(NotFoundErrors))]
    public async Task PostValidateEmailSendAsync_CenarioDeErro_RetornaNotFound(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var request = new PostValidateEmailSendRequestBuilder();
        var notFoundResult = BuildNotFound(enumError);

        MockService.PostValidateEmailSendAsync(Arg.Any<PostValidateEmailSendRequest>())
                   .Returns(Task.FromResult<IActionResult>(notFoundResult));

        // Act
        var result = await Controller.PostValidateEmailSendAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region Service Delegation

    [Fact(DisplayName = "Reenvio do token deve delegar a chamada ao service exatamente uma vez")]
    public async Task PostValidateEmailSendAsync_QuandoChamado_DeveDelegarAoService()
    {
        // Arrange
        var request = new PostValidateEmailSendRequestBuilder();
        var okResult = BuildOk(EnumErrosApi.POSTVALIDATEEMAILSENDASYNC_AUTHSERVICE_200_TOKEN_CREATED.GetDescription());

        MockService.PostValidateEmailSendAsync(Arg.Any<PostValidateEmailSendRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        await Controller.PostValidateEmailSendAsync(request);

        // Assert
        await MockService.Received(1).PostValidateEmailSendAsync(Arg.Any<PostValidateEmailSendRequest>());
        Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
    }

    #endregion
}
