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

public class PostCreateUserSsoAsyncTest : UsersTestBase
{
    public PostCreateUserSsoAsyncTest(ITestOutputHelper output) : base(output) { }

    #region Success

    [Fact(DisplayName = "Criação de usuário SSO deverá ser feita com sucesso (Novo Usuário)")]
    public async Task PostCreateUserSsoAsync_DadosValidosNovoUsuario_RetornaCreated()
    {
        // Arrange
        var request = new PostCreateUserSsoRequestBuilder();
        var createdResult = new StatusCodeResult(201);

        MockService.PostCreateUserSsoAsync(Arg.Any<PostCreateUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(createdResult));

        // Act
        var result = await Controller.PostCreateUserSsoAsync("TestAgent/1.0", "localhost", request);

        // Assert
        result.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(201);
        Output.WriteLine($"\n Validado criação (201) para provedor {request.Provider} \n");
    }

    [Fact(DisplayName = "Criação de usuário SSO deverá retornar OK (Usuário já existe)")]
    public async Task PostCreateUserSsoAsync_UsuarioJaExiste_RetornaOk()
    {
        // Arrange
        var request = new PostCreateUserSsoRequestBuilder();
        var okResult = BuildOk(new { Message = EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_200_USER_ALREADY_EXISTS.GetDescription() });

        MockService.PostCreateUserSsoAsync(Arg.Any<PostCreateUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        var result = await Controller.PostCreateUserSsoAsync("TestAgent/1.0", "localhost", request);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;
        responseValue.Should().NotBeNull();
        Output.WriteLine($"\n Validado usuário existente (200) para provedor {request.Provider} \n");
    }

    #endregion

    #region BadRequest Errors

    public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
    {
        new object[] { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NULL_OR_EMPTY, "Provedor nulo ou vazio" },
        new object[] { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_CONFIGURED, "Provedor não configurado" },
        new object[] { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_INACTIVE, "Provedor inativo" },
        new object[] { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_NOT_200_RESPONSE, "Provedor não retornou 200" },
        new object[] { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_NULL_OR_EMPTY, "Token de acesso nulo ou vazio" },
        new object[] { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_PROVIDER_ACCESS_TOKEN_ID_INVALID, "Token de acesso inválido" },
        new object[] { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_HOST_NULL_OR_EMPTY, "Host vazio ou nulo" },
        new object[] { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_USER_AGENT_NULL_OR_EMPTY, "User-Agent vazio ou nulo" },
        new object[] { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_TERMS_NOT_ACCEPTED, "Termos não aceitos" },
        new object[] { EnumErrosApi.POSTCREATEUSERSSOASYNC_AUTHSERVICE_400_ERROR_WHILE_ACCEPT_TERMS, "Erro ao registrar aceite dos termos" },
    };

    [Theory(DisplayName = "Criação de usuário SSO deverá retornar BadRequest para cenários de erro")]
    [MemberData(nameof(BadRequestErrors))]
    public async Task PostCreateUserSsoAsync_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
    {
        // Arrange
        var request = new PostCreateUserSsoRequestBuilder();
        var badRequestResult = BuildBadRequest(enumError);

        MockService.PostCreateUserSsoAsync(Arg.Any<PostCreateUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(badRequestResult));

        // Act
        var result = await Controller.PostCreateUserSsoAsync("TestAgent/1.0", "localhost", request);

        // Assert
        var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;

        responseValue.Messages.Should().Contain(enumError.GetDescription());
        Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
    }

    #endregion

    #region Service Delegation

    [Fact(DisplayName = "Criação de usuário SSO deve delegar a chamada ao service exatamente uma vez")]
    public async Task PostCreateUserSsoAsync_QuandoChamado_DeveDelegarAoService()
    {
        // Arrange
        var request = new PostCreateUserSsoRequestBuilder();
        var okResult = new StatusCodeResult(201);

        MockService.PostCreateUserSsoAsync(Arg.Any<PostCreateUserSsoRequest>())
                   .Returns(Task.FromResult<IActionResult>(okResult));

        // Act
        await Controller.PostCreateUserSsoAsync("TestAgent/1.0", "localhost", request);

        // Assert
        await MockService.Received(1).PostCreateUserSsoAsync(Arg.Any<PostCreateUserSsoRequest>());
        Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
    }

    #endregion
}
