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

public class PatchResetPasswordAsyncTest : AuthenticatorTestBase
{
    public PatchResetPasswordAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "Reset de senha deverá ser feito com sucesso")]
    public async Task PatchResetPasswordAsync_DadosValidos_RetornaSucesso()
    {
        // Arrange
        var request = new PatchResetPasswordRequestBuilder();
        var okResult = BuildOk(new { Message = EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_200_PASSWORD_CHANGED.GetDescription() });

        MockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        var result = await Controller.PatchResetPasswordAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;

        responseValue.Should().NotBeNull();
        Output.WriteLine($"\n Validado sucesso ao resetar a senha do usuário com email {request.Email} \n");
    }

    #endregion

    #region BadRequest Errors

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_INVALID_EMAIL, "Email inválido" },
        new object[] { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_EMAIL_NULL_OR_EMPTY, "Email nulo ou vazio" },
        new object[] { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_TOKEN_INVALID, "Token inválido" },
        new object[] { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_EMPTY_NULL, "Senha nula ou vazia" },
        new object[] { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_WITH_WRONG_LENGHT, "Tamanho incorreto da senha" },
        new object[] { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_WRONG_FORMAT, "Formato incorreto da senha" },
        new object[] { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_IS_THE_SAME, "Mesma senha" },
        new object[] { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_400_PASSWORD_NOT_UPDATED, "Falha ao atualizar senha" },
    };

    [Theory(DisplayName = "Reset de senha deverá retornar BadRequest para cenários de erro")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task PatchResetPasswordAsync_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var request = new PatchResetPasswordRequestBuilder();
        var badRequestResult = BuildBadRequest(enumError);

        MockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        // Act
        var result = await Controller.PatchResetPasswordAsync(request);

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
        new object[] { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_404_USER_NOT_FOUNND, "Usuário não encontrado" },
        new object[] { EnumErrosApi.PATCHRESETPASSWORDASYNC_AUTHSERVICE_404_USER_TOKEN_NOT_FOUND_IN_CACHE, "Token não encontrado no cache" },
    };

    [Theory(DisplayName = "Reset de senha deverá retornar NotFound para cenários de erro")]
    [MemberData(nameof(NotFoundErrors))]
    public async Task PatchResetPasswordAsync_CenarioDeErro_RetornaNotFound(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var request = new PatchResetPasswordRequestBuilder();
        var notFoundResult = BuildNotFound(enumError);

        MockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(notFoundResult));

        // Act
        var result = await Controller.PatchResetPasswordAsync(request);

        // Assert
        var objectResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region Service Delegation

    [Fact(DisplayName = "Reset de senha deve delegar a chamada ao service exatamente uma vez")]
    public async Task PatchResetPasswordAsync_QuandoChamado_DeveDelegarAoService()
    {
        // Arrange
        var request = new PatchResetPasswordRequestBuilder();
        var okResult = BuildOk(new { Message = "Password changed" });

        MockService.PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        await Controller.PatchResetPasswordAsync(request);

        // Assert — verifica que o service foi chamado exatamente 1 vez
        await MockService.Received(1).PatchResetPasswordAsync(Arg.Any<PatchResetPasswordRequest>());
        Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
    }

    #endregion
}
