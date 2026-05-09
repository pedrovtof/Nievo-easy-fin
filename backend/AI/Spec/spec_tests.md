# Especificação de Testes - Nievo Easyfin

## 1. Objetivos

Garantir a integridade das regras de negócio, validações de API e integração com provedores externos (SSO/SMTP), mantendo uma cobertura de código robusta e testes fáceis de manter.

## 2. Tecnologias e Ferramentas

- **xUnit:** Runner de testes.
- **FluentAssertions:** Legibilidade nas asserções.
- **NSubstitute:** Mocking de interfaces e serviços.
- **Bogus:** Geração de dados randômicos e realistas.
- **Moq.EntityFrameworkCore:** Mocking de `DbSet` e `DbContext`.

## 3. Padrões e Convenções

### 3.1 Nomenclatura de Arquivos e Classes

- Os arquivos de teste devem espelhar a estrutura do projeto principal.
- Exemplo: `NievoEasyfin.Application/Services/Base/UsersService.cs` -> `NievoEasyfin.Tests/Application/Services/Base/UsersServiceTests.cs`.

### 3.2 Nomenclatura de Métodos (Padrão: Objeto_Estado_Resultado)

- Exemplo: `PostCreateUserAsync_WithValidRequest_ReturnsCreated`
- Exemplo: `PostCreateUserAsync_WhenEmailExists_ReturnsBadRequest_EmailEmptY`

### 3.3 Estrutura do Teste (AAA)

- **Arrange:** Configuração de mocks, instâncias e dados.
- **Act:** Chamada do método a ser testado.
- **Assert:** Verificação do resultado e comportamentos esperados.

## 4. Estratégia de Mocking

- Utilizar `NSubstitute` para todas as dependências de serviço.
- Criar classes de `Faker` (Bogus) em `NievoEasyfin.Tests/Mocks/Fakers` para entidades e requests comuns.

## 5. Casos de Teste Iniciais (Exemplo: UsersService)

### PostCreateUserAsync

1. **Sucesso:** Retorna 201 quando os dados são válidos e o e-mail não existe.
2. **Falha de Validação:** Retorna 400 quando o request é inválido (ex: e-mail vazio).
3. **E-mail Duplicado:** Retorna 400 quando o e-mail já está cadastrado no banco.
4. **Erro Interno:** Validar comportamento caso o hasher de senha falhe.

## 6. Testes de Integração

- Localizados em `NievoEasyfin.Tests/API/`.
- Devem testar o pipeline completo (Controller -> Service -> Database/Mocks).
- Utilizar `WebApplicationFactory` se necessário para subir o ambiente de teste.

## 7. Próximos Passos

1. Implementar `UsersServiceTests.cs`.
2. Criar `UserFaker` e `PostCreateUserRequestFaker`.
3. Configurar base de mocks para `UserModel`.

## 8. Regras

1. Não se pode editar qualquer projeto que não seja o `NievoEasyFin.Tests`.
2. Extritamente proibido criar novas regras de negocio.
3. Em caso de restrições por estrutura atual do projeto, crie um arquivo no proprio projeto de testes que herde e simule o real.
