# Spec — Testes Unitários para Endpoints

> **Projeto:** NievoEasyfin  
> **Última atualização:** 2026-05-06  
> **Stack de testes:** xUnit 2.9 · FluentAssertions 8.9 · NSubstitute 6.0 · Bogus 35.6

---

## 1. Visão Geral da Arquitetura de Testes

```
NievoEasyfin.Tests/
├── API/                          # Testes de endpoints (Controller-level)
│   └── {NomeController}/        # Ex: Auth
│       ├── Public/              # Endpoints públicos
│       │   ├── {Controller}TestBase.cs
│       │   ├── {MetodoEndpoint}Test.cs
│       │   └── ...
│       └── Admin/               # Endpoints privados/admin (futuro)
├── Build/                        # Infraestrutura de construção de dados
│   ├── Generators/              # Utilitários de geração (ex: PasswordGenerator)
│   └── Request/                 # Builders fluentes para requests
├── Contracts/                    # Testes de contrato (futuro)
├── Mocks/                        # Mocks reutilizáveis (futuro)
└── test.csproj
```

**Regra de ouro:** A estrutura de pastas dos testes **espelha** a estrutura dos controllers.

- Controller em `NievoEasyfin.Auth/Controllers/Public/AuthenticatorController.cs`
- Testes em `NievoEasyfin.Tests/API/Auth/Public/`

---

## 2. Dependências (test.csproj)

```xml
<PackageReference Include="Bogus" Version="35.6.5" />
<PackageReference Include="coverlet.collector" Version="6.0.4" />
<PackageReference Include="FluentAssertions" Version="8.9.0" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
<PackageReference Include="NSubstitute" Version="6.0.0-rc.1" />
<PackageReference Include="xunit" Version="2.9.3" />
<PackageReference Include="xunit.runner.visualstudio" Version="3.1.4" />
```

| Pacote | Função |
|---|---|
| **xUnit** | Framework de testes (Fact, Theory, MemberData) |
| **FluentAssertions** | Asserções legíveis (`.Should().BeOfType<>()`) |
| **NSubstitute** | Mocking de interfaces de serviço |
| **Bogus** | Geração de dados fake realistas (pt_BR) |
| **coverlet** | Cobertura de código |

---

## 3. Pré-requisito: Interface de Serviço

O controller **DEVE** depender de uma **interface** (`IXxxService`), não da classe concreta. Isso é obrigatório para mocking.

```csharp
// ✅ Correto — controller depende da interface
public class AuthenticatorController : Controller
{
    private readonly IAuthenticatorService _authenticatorService;
    public AuthenticatorController(IAuthenticatorService authenticatorService) { ... }
}

// ❌ Errado — controller depende da classe concreta (não permite mock)
public class UsersController : Controller
{
    private readonly UsersService _usersService; // BLOQUEIA TESTES
}
```

**Se o controller usar classe concreta:** crie a interface primeiro (`IXxxService`), extraia os métodos, e refatore o controller para usar a interface. Registre no DI (`Startup.cs`).

---

## 4. TestBase — Classe Base por Controller

Cada controller deve ter **uma classe base abstrata** que centraliza:

- Mock do serviço
- Instância do controller
- `ITestOutputHelper` para logs
- Helpers para construir responses (`BuildOk`, `BuildBadRequest`, `BuildNotFound`)

### Template

```csharp
using Microsoft.AspNetCore.Mvc;
using NievoEasyfin.Application.Extensions.Enum;
using NievoEasyfin.Application.Interfaces.Enum;
using NievoEasyfin.Application.Interfaces.Response;
using NievoEasyfin.Application.Interfaces.Services;
using NievoEasyfin.Auth.Controllers.Public; // ou Admin
using NSubstitute;
using Xunit.Abstractions;

namespace NievoEasyfin.Tests.API.{Area}.{Visibilidade};

/// <summary>
/// Base class for all {Controller}Controller tests.
/// </summary>
public abstract class {Controller}TestBase
{
    protected readonly I{Service}Service MockService;
    protected readonly {Controller}Controller Controller;
    protected readonly ITestOutputHelper Output;

    protected {Controller}TestBase(ITestOutputHelper output)
    {
        Output = output;
        MockService = Substitute.For<I{Service}Service>();
        Controller = new {Controller}Controller(MockService);
    }

    protected static BadRequestObjectResult BuildBadRequest(EnumErrosApi enumError)
    {
        var response = new ResponseApiError(new List<string> { enumError.GetDescription() });
        return new BadRequestObjectResult(response);
    }

    protected static NotFoundObjectResult BuildNotFound(params EnumErrosApi[] enumErrors)
    {
        var messages = enumErrors.Select(e => e.GetDescription()).ToList();
        var response = new ResponseApiError(messages);
        return new NotFoundObjectResult(response);
    }

    protected static OkObjectResult BuildOk(object data)
    {
        var response = new ResponseApiSucess(data);
        return new OkObjectResult(response);
    }
}
```

---

## 5. Request Builders — Pattern Fluente

Cada `Request` model do endpoint deve ter um **Builder** em `Build/Request/`.

### Regras

1. O Builder **herda** da classe Request original (ex: `PostLoginUserRequest`)
2. O construtor preenche **todos os campos com dados válidos** via Bogus
3. Métodos `With{Propriedade}()` permitem sobrescrever valores específicos
4. Retorna `this` para encadeamento fluente
5. Locale do Bogus: `"pt_BR"`

### Template

```csharp
using Bogus;
using NievoEasyfin.Application.Interfaces.Request;
using NievoEasyfin.Tests.Build.Generators; // se necessário

namespace NievoEasyfin.Tests.Build.Request;

/// <summary>
/// Fluent builder for {RequestClass}.
/// Default values are set in the constructor — no need to call Default().
/// </summary>
public class {RequestClass}Builder : {RequestClass}
{
    private readonly Faker _faker = new Faker("pt_BR");

    public {RequestClass}Builder()
    {
        // Preencher TODOS os campos com dados válidos
        Email = _faker.Person.Email;
        Password = PasswordGenerator.Generate(); // se aplicável
        // ... demais campos
    }

    public {RequestClass}Builder WithEmail(string email)
    {
        Email = email;
        return this;
    }

    // Repetir para cada propriedade
}
```

### Exemplo Real — `PatchResetPasswordRequestBuilder`

```csharp
public class PatchResetPasswordRequestBuilder : PatchResetPasswordRequest
{
    private readonly Faker _faker = new Faker("pt_BR");

    public PatchResetPasswordRequestBuilder()
    {
        Email = _faker.Person.Email;
        PinToken = _faker.Random.Number(100000, 999999).ToString();
        Password = PasswordGenerator.Generate();
    }

    public PatchResetPasswordRequestBuilder WithEmail(string email) { Email = email; return this; }
    public PatchResetPasswordRequestBuilder WithPinToken(string pinToken) { PinToken = pinToken; return this; }
    public PatchResetPasswordRequestBuilder WithPassword(string password) { Password = password; return this; }
}
```

---

## 6. Generators — Utilitários de Geração

Dados complexos com regras de validação (ex: senhas) devem ser centralizados em `Build/Generators/`.

### PasswordGenerator (referência)

```csharp
using System.Text;
using Bogus;

namespace NievoEasyfin.Tests.Build.Generators;

public static class PasswordGenerator
{
    private static readonly Faker Faker = new Faker("pt_BR");
    private static readonly List<string> Symbols = new()
    {
        "!", "@", "#", "$", "%", "^", "&", "*", "(", ")",
        "+", "=", "/", "[", "]", "{", "}", "\\", "`", "~",
        "<", ">", ",", "."
    };

    public static string Generate()
    {
        var str = new StringBuilder();
        str.Append(Faker.Hacker.Random.AlphaNumeric(3).ToUpper());
        str.Append(Faker.Random.AlphaNumeric(3));
        str.Append(Faker.Random.Number(3).ToString());
        str.Append(string.Join("", Faker.PickRandom(Symbols, Faker.Random.Number(1, 3))));
        return str.ToString();
    }
}
```

---

## 7. Estrutura de um Arquivo de Teste

Cada endpoint tem **um arquivo de teste dedicado** com 4 `#region`:

```
{MetodoEndpoint}Test.cs
├── #region Success           → [Fact] cenário feliz
├── #region BadRequest Errors → [Theory] + MemberData para 400
├── #region NotFound Errors   → [Theory] ou [Fact] para 404
└── #region Service Delegation → [Fact] verifica chamada ao service
```

### 7.1 Nomenclatura

| Item | Padrão | Exemplo |
|---|---|---|
| **Arquivo** | `{MetodoEndpoint}Test.cs` | `PostLoginUserAsyncTest.cs` |
| **Classe** | `{MetodoEndpoint}Test` | `PostLoginUserAsyncTest` |
| **Método Fact** | `{Metodo}_{Condicao}_{ResultadoEsperado}` | `PostLoginUserAsync_DadosValidos_RetornaSucesso` |
| **Método Theory** | `{Metodo}_CenarioDeErro_Retorna{StatusCode}` | `PostLoginUserAsync_CenarioDeErro_RetornaBadRequest` |
| **DisplayName** | Descrição em português | `"Login deverá ser feito com sucesso"` |

### 7.2 Padrão AAA (Arrange-Act-Assert)

Todo teste segue rigorosamente:

```csharp
// Arrange — preparar request e configurar mock
var request = new {Request}Builder();
var okResult = BuildOk(new { Token = "mocked-jwt-token" });

MockService.{Metodo}(Arg.Any<{RequestType}>())
           .Returns(Task.FromResult<IActionResult>(okResult));

// Act — executar o endpoint
var result = await Controller.{Metodo}(request);

// Assert — validar resultado
var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
var responseValue = objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject;
responseValue.Should().NotBeNull();

// Log para diagnóstico
Output.WriteLine($"\n Validado sucesso com {request.Email} \n");
```

---

## 8. Cenários Obrigatórios por Endpoint

### 8.1 Sucesso (`[Fact]`)

```csharp
[Fact(DisplayName = "{Descrição} deverá ser feito com sucesso")]
public async Task {Metodo}_DadosValidos_RetornaSucesso()
{
    var request = new {Request}Builder();
    var okResult = BuildOk(new { /* dados mock */ });

    MockService.{Metodo}(Arg.Any<{RequestType}>())
               .Returns(Task.FromResult<IActionResult>(okResult));

    var result = await Controller.{Metodo}(request);

    var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
    objectResult.Value.Should().BeOfType<ResponseApiSucess>().Subject.Should().NotBeNull();
    Output.WriteLine($"\n Validado sucesso ... \n");
}
```

### 8.2 BadRequest — Testes Parametrizados (`[Theory]`)

```csharp
public static IEnumerable<object[]> BadRequestErrors => new List<object[]>
{
    new object[] { EnumErrosApi.{ENUM_400_1}, "Descrição cenário 1" },
    new object[] { EnumErrosApi.{ENUM_400_2}, "Descrição cenário 2" },
};

[Theory(DisplayName = "{Descrição} deverá retornar BadRequest para cenários de erro")]
[MemberData(nameof(BadRequestErrors))]
public async Task {Metodo}_CenarioDeErro_RetornaBadRequest(EnumErrosApi enumError, string cenario)
{
    var request = new {Request}Builder();
    var badRequestResult = BuildBadRequest(enumError);

    MockService.{Metodo}(Arg.Any<{RequestType}>())
               .Returns(Task.FromResult<IActionResult>(badRequestResult));

    var result = await Controller.{Metodo}(request);

    var objectResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
    var responseValue = objectResult.Value.Should().BeOfType<ResponseApiError>().Subject;
    responseValue.Messages.Should().Contain(enumError.GetDescription());
    Output.WriteLine($"\n Validado erro: {cenario} ({enumError}) \n");
}
```

### 8.3 NotFound (`[Theory]` ou `[Fact]`)

Usar `[Theory]` com `MemberData` se houver múltiplos cenários 404. Usar `[Fact]` se houver apenas 1.

```csharp
// Múltiplos cenários 404 → Theory
public static IEnumerable<object[]> NotFoundErrors => new List<object[]>
{
    new object[] { EnumErrosApi.{ENUM_404_1}, "Cenário 1" },
};

[Theory(DisplayName = "...")]
[MemberData(nameof(NotFoundErrors))]
public async Task {Metodo}_CenarioDeErro_RetornaNotFound(EnumErrosApi enumError, string cenario)
{
    var request = new {Request}Builder();
    var notFoundResult = BuildNotFound(enumError);
    // ... mesmo padrão, com NotFoundObjectResult
}
```

### 8.4 Service Delegation (`[Fact]`)

Valida que o controller delegou a chamada ao serviço **exatamente 1 vez**.

```csharp
[Fact(DisplayName = "{Descrição} deve delegar a chamada ao service exatamente uma vez")]
public async Task {Metodo}_QuandoChamado_DeveDelegarAoService()
{
    var request = new {Request}Builder();
    var okResult = BuildOk(new { /* dados */ });

    MockService.{Metodo}(Arg.Any<{RequestType}>())
               .Returns(Task.FromResult<IActionResult>(okResult));

    await Controller.{Metodo}(request);

    await MockService.Received(1).{Metodo}(Arg.Any<{RequestType}>());
    Output.WriteLine("\n Validado que o service foi chamado exatamente 1 vez. \n");
}
```

---

## 9. Como Identificar os Cenários de Erro

Os cenários são extraídos do `EnumErrosApi`. A convenção do enum é:

```
{METODO}_{SERVICE}_{STATUS_CODE}_{DESCRICAO}
```

Exemplos:

- `POSTLOGINUSERASYNC_AUTHSERVICE_400_EMAIL_EMPTY_NULL` → BadRequest
- `POSTLOGINUSERASYNC_AUTHSERVICE_404_USER_NOT_FOUND` → NotFound

**Processo:**

1. Abra `EnumErrosApi.cs`
2. Filtre pelo prefixo do método (ex: `POSTLOGINUSERASYNC_`)
3. Agrupe por status code (`400` → BadRequest, `404` → NotFound)
4. Ignore os de sucesso (`200`, `201`)
5. Cada erro vira uma entrada no `MemberData`

---

## 10. Checklist — Criando Testes para um Novo Endpoint

```
□ 1. Verificar se o controller usa INTERFACE (não classe concreta)
      → Se não, criar a interface e refatorar
□ 2. Verificar se o TestBase do controller já existe
      → Se não, criar seguindo o template da Seção 4
□ 3. Criar o RequestBuilder em Build/Request/
      → Herdar da classe Request, preencher com Bogus
      → Se precisa de senha, usar PasswordGenerator
□ 4. Criar o arquivo de teste em API/{Area}/{Visibilidade}/
□ 5. Implementar os 4 cenários obrigatórios:
      □ 5a. Success [Fact]
      □ 5b. BadRequest [Theory] com MemberData (filtrar EnumErrosApi _400_)
      □ 5c. NotFound [Theory/Fact] (filtrar EnumErrosApi _404_)
      □ 5d. Service Delegation [Fact]
□ 6. Rodar os testes: dotnet test
□ 7. Verificar que todos passam
```

---

## 11. Comandos Úteis

```bash
# Rodar todos os testes
dotnet test

# Rodar testes com output detalhado
dotnet test --logger "console;verbosity=detailed"

# Rodar testes de um arquivo/classe específica
dotnet test --filter "FullyQualifiedName~PostLoginUserAsyncTest"

# Rodar com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

---

## 12. Boas Práticas

| Prática | Detalhe |
|---|---|
| **1 arquivo = 1 endpoint** | Nunca misturar testes de endpoints diferentes |
| **Builder no construtor** | Dados válidos por padrão, sem `Default()` |
| **`Output.WriteLine`** | Sempre logar o resultado para diagnóstico |
| **`Arg.Any<T>()`** | Usar para matcher genérico no mock |
| **`Task.FromResult<IActionResult>`** | Sempre tipar explicitamente o retorno do mock |
| **DisplayName em pt-BR** | Facilita leitura no test runner |
| **Métodos em en-US** | Nomes de métodos e classes seguem inglês |
| **Não testar lógica de negócio** | Os testes de controller validam delegação e contrato HTTP |
| **FluentAssertions** | Sempre usar `.Should()` — nunca `Assert.Equal()` |
| **`#region`** | Organizar por tipo de cenário |

---

## 13. Pasta `/AI/Cache` — Contexto para a IA

A pasta `AI/Cache/` está no `.gitignore` e **NÃO sobe para o GitHub**.

Use essa pasta para criar seus proprios comentarios e usar para Cache interno, evitando releitura de codigo.

### Propósito

Armazenar **exemplos completos, snapshots de código e contexto** que a IA pode consultar para manter consistência ao criar novos testes.

### O que salvar lá

- Cópias completas de arquivos de teste já finalizados (como referência)
- Cópias de builders e generators
- Snapshots do `EnumErrosApi.cs` para consulta rápida
- Qualquer outro contexto útil para a IA gerar código consistente

### Como usar

Ao pedir para a IA criar testes para um novo endpoint, **referencie a pasta**:

> "Crie testes para o endpoint `PostCreateUserAsync` seguindo os padrões documentados em `AI/Spec/spec_testes_endpoints.md` e usando os exemplos em `AI/Cache/` como referência."

---

## 14. Exemplo Completo de Referência

Para um exemplo real e completo, veja os arquivos de cache em `AI/Cache/`:

- `AI/Cache/exemplo_teste_completo.cs` — Teste completo do `PostLoginUserAsyncTest`
- `AI/Cache/exemplo_builder.cs` — Builder do `PostLoginUserRequestBuilder`
- `AI/Cache/exemplo_testbase.cs` — TestBase do `AuthenticatorController`
- `AI/Cache/exemplo_generator.cs` — `PasswordGenerator`
- `AI/Cache/snapshot_enum_erros.cs` — Snapshot do `EnumErrosApi`
