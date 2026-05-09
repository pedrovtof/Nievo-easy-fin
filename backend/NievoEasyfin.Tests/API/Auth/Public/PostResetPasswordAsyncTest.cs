using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Extensions.Enum;
using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Tests.Build.Request;
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyfin.Tests.API.Auth.Public;

public class PostResetPasswordAsyncTest : AuthenticatorTestBase
{
    public PostResetPasswordAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "Solicitação de reset de senha deverá ser feita com sucesso")]
    public async Task PostResetPasswordAsync_DadosValidos_RetornaSucesso()
    {
        // Arrange
        var request = new PostResetPasswordRequestBuilder();
        var okResult = BuildOk(new { Message = "Token sent" });

        MockService.PostResetPasswordAsync(Arg.Any<PostResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        var result = await Controller.PostResetPasswordAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;

        responseValue.Should().NotBeNull();
        Output.WriteLine($"\n Validado sucesso ao solicitar reset de senha para o email {request.Email} \n");
    }

    #endregion

    #region BadRequest Errors

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAIL, "Email inválido" },
        new object[] { EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTY, "Email nulo ou vazio" },
        new object[] { EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_400_USER_TOKEN_FOUND_IN_CACHE, "Token já gerado em cache" },
    };

    [Theory(DisplayName = "Solicitação de reset de senha deverá retornar BadRequest para cenários de erro")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task PostResetPasswordAsync_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var request = new PostResetPasswordRequestBuilder();
        var badRequestResult = BuildBadRequest(enumError);

        MockService.PostResetPasswordAsync(Arg.Any<PostResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        // Act
        var result = await Controller.PostResetPasswordAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region NotFound Errors

    [Fact(DisplayName = "Solicitação de reset de senha deverá retornar erro de usuário não encontrado")]
    public async Task PostResetPasswordAsync_UsuarioNaoEncontrado_RetornaNotFound()
    {
        // Arrange
        var request = new PostResetPasswordRequestBuilder();
        var notFoundResult = BuildNotFound(EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUNND);

        MockService.PostResetPasswordAsync(Arg.Any<PostResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(notFoundResult));

        // Act
        var result = await Controller.PostResetPasswordAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(EnumErrosApi.POSTRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUNND.GetDescription());
        Output.WriteLine($"\n Validado erro de usuário não encontrado. \n");
    }

    #endregion

    #region Service Delegation

    [Fact(DisplayName = "Reset de senha deve delegar a chamada ao service exatamente uma vez")]
    public async Task PostResetPasswordAsync_QuandoChamado_DeveDelegarAoService()
    {
        // Arrange
        var request = new PostResetPasswordRequestBuilder();
        var okResult = BuildOk(new { Message = "Token sent" });

        MockService.PostResetPasswordAsync(Arg.Any<PostResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        await Controller.PostResetPasswordAsync(request);

        // Assert — verifica que o service foi chamado exatamente 1 vez
        await MockService.Received(1).PostResetPasswordAsync(Arg.Any<PostResetPasswordRequest>());
        Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
    }

    #endregion
}
