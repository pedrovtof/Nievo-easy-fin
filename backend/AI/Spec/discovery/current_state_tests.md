# Discovery — Estado Atual dos Testes e Dependências

> **Projeto:** NievoEasyfin  
> **Data:** 2026-05-09

## 1. Problemas Identificados

### 1.1 Inviabilidade de Mocks Unitários
O `AuthenticatorService` depende de classes concretas em vez de interfaces:
- `CryptoPasswordService`
- `AuthDbCacheService`
- `UserModel`
- `UserProviderSsoModel`
- `JsonWebTokenService`
- `SSoProviderAuth`
- `SmtpModel`

Como os métodos nessas classes não são `virtual`, o `NSubstitute` não consegue interceptar as chamadas. Para testar o `AuthenticatorService`, ele acaba executando a lógica real de todas essas dependências, o que não é um teste unitário.

### 1.2 Dependência de Variáveis de Ambiente Estáticas
A classe `CryptoPasswordService` utiliza `DotNetEnv.Env` em campos estáticos:
```csharp
private static readonly int Iterations = DotNetEnv.Env.GetInt("PASSWORD_CRYPTO_ITERATIONS");
```
Isso dificulta a configuração de diferentes cenários de teste e pode causar falhas se o arquivo `.env` não estiver presente no ambiente de testes.

### 1.3 Acoplamento em `UserModel`
`UserModel` herda de `UserEntity` e depende diretamente de `AuthOrigin` e `AuthReplica` (Contextos do EF). Além disso, utiliza Dapper para queries complexas.

## 2. Necessidades de Refatoração

Para permitir testes que evitem bugs, precisamos:
1. **Abstrair Dependências**: Criar interfaces para todos os serviços e modelos injetados no `AuthenticatorService`.
2. **Injeção de Dependência**: Atualizar o construtor do `AuthenticatorService` para aceitar essas interfaces.
3. **Configuração de Testes de Integração**: Criar uma infraestrutura para rodar testes contra um banco SQLite em memória para validar o `UserModel` (Dapper + EF).

## 3. Próximos Passos
- Criar `I{Service}` para as dependências.
- Refatorar `AuthenticatorService`.
- Implementar `AuthenticatorServiceTest` (Unitário).
- Implementar `UserModelIntegrationTest` (Integração).
