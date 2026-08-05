# Especificação de Testes - Nievo Easyfin

## 1. Objetivos

Garantir a integridade das regras de negócio, validações de API e integração com provedores externos (SSO/SMTP/Cache), mantendo uma cobertura de código robusta e testes fáceis de manter.

## 2. Tecnologias e Ferramentas

- **xUnit:** Runner de testes e framework de testes principal.
- **FluentAssertions:** Asserções fluentes para validações mais legíveis e descritivas.
- **Moq:** Framework de mocking utilizado para interfaces, serviços de cache (`IDatabase`), e simulação comportamental.
- **Bogus:** Geração de dados randômicos e realistas para preenchimento de *requests* e entidades do banco (*Fakers*).
- **SQLite (In-Memory):** Utilizado via `DbContextMockFactory` para simular os bancos relacionais reais de *origin* e *replica* do EF Core sem necessidade de container.
- **WireMock.Net:** Simulação de APIs HTTP e requisições externas, acoplado via `WireMockFixture`.
- **DotNetEnv:** Carregamento de variáveis de ambiente locais (`.env`) para o ambiente de execução dos testes.

## 3. Padrões de Estrutura e Diretórios

Diferente de estruturas puramente espelhadas, os testes em `NievoEasyFin.Tests` são direcionados aos endpoints da API e isolamento de dependências.

### 3.1 Diretórios Principais

- **`API/`**: Testes da aplicação organizados pelo contexto e nível de acesso. (Ex: `API/Auth/Public/`, `API/Core/Private/`).
  - As classes de teste representam o endpoint/ação a ser testado (Ex: `PostCreateUserAsyncTest.cs`).
  - Cada contexto possui uma classe base de configuração, como `UsersServiceTestBase.cs`, que injeta serviços fundamentais.
- **`Build/Request/`**: Classes no padrão *Builder* para facilitar e padronizar a montagem de DTOs e requests complexos (Ex: `PostCreateUserRequestBuilder.cs`).
- **`Mocks/`**: Centralização da infraestrutura e simulações.
  - `Database/`: Mocks de banco de dados, contendo a fábrica de SQLite (`DbContextMockFactory.cs`).
  - `Fakers/`: Implementações de geração de dados via `Bogus` para entidades (`UserEntityFaker.cs`) e requisições.
  - `Helpers/` & `Infrastructure/`: Utilitários, mocks manuais (`SmtpModelMock.cs`), factory para mocks comuns (Cache/Redis) e o setup global do WireMock.

### 3.2 Nomenclatura de Métodos (Padrão: Ação_Condição_Resultado)

- Exemplo: `PostCreateUserAsync_WithValidRequest_ReturnsCreated`
- Exemplo: `PostCreateUserAsync_WhenEmailExistsWithActiveStatus_ReturnsBadRequest`

Todos os testes devem utilizar a anotação `[Fact(DisplayName = "...")]` relatando detalhadamente o caso.

### 3.3 Estrutura do Teste e Escopo

- **Region Grouping:** Os testes devem ser explicitamente agrupados em blocos `#region Success` ou `#region BadRequest Errors`.
- **Padrão AAA:** (Arrange, Act, Assert).
  - *Arrange*: Usar classes `RequestBuilder`, criar o mock do banco com `CreateSharedAuthContexts()` e instanciar os serviços.
  - *Act*: Invocar a chamada em teste rodeada com tratamentos para exceptions intencionais se necessário.
  - *Assert*: Validar os `ObjectResult` de retorno, checar códigos HTTP (ex: 201, 400) e verificar no banco simulado de leitura (`replica`) se os dados foram persistidos conforme esperado.
- **Logging:** Deve-se registrar as etapas dos testes usando `Output.WriteLine("Mensagem");` (via `ITestOutputHelper`) visando facilitar depuração.

## 4. Estratégia de Mocking e Injeção

- **Banco de Dados:** Utilizar o método `DbContextMockFactory.CreateSharedAuthContexts()` para obter e configurar os contextos transacionais e de réplica (`AuthOrigin`, `AuthReplica`) baseados no SQLite.
- **Construção de Requests:** É altamente recomendado o uso das classes em `Build/Request/` para não poluir os arquivos de teste construindo propriedades do zero, salvo os campos essenciais para variação de cenários.
- **Dependências Externas:** Ao invés de acessar rede e banco, todas as conexões como Redis e chamadas HTTP devem ser devidamente injetadas usando os métodos em `MockHelper` e as rotas mockadas com o `_wireMockServer` disponível nas classes bases de teste.

## 5. Regras Importantes

1. Modificações limitam-se ao escopo do `NievoEasyFin.Tests`. Alterações em outros projetos exigem confirmação.
2. É estritamente proibido criar lógicas ou alterar regras de negócio do código fonte original apenas para contornar falhas nos testes.
3. Se um bloqueio ou restrição de design surgir no projeto, prefira utilizar a herança e mock manual nas pastas de suporte (`Mocks/Infrastructure/` ou classes aninhadas) para contornar o problema, ao invés de alterar o comportamento produtivo de serviços reais.
