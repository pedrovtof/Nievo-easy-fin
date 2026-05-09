# Design — Testes Unitários de Service e Integração

> **Projeto:** NievoEasyfin  
> **Data:** 2026-05-09

## 1. Estratégia de Testes Unitários (Services)

### 1.1 Objetivo
Validar a lógica de negócio contida nos Services (ex: `AuthenticatorService`), isolando-os de bancos de dados, cache e serviços externos.

### 1.2 Mudanças Estruturais
- Criar interfaces em `NievoEasyfin.Application/Interfaces/Services/` (já existem algumas, mas faltam para modelos e helpers).
- Injetar interfaces no `AuthenticatorService`.

### 1.3 Estrutura de Teste
```
NievoEasyfin.Tests/
├── Services/
│   ├── Base/
│   │   ├── ServiceTestBase.cs (Setup de mocks comuns)
│   │   └── AuthenticatorServiceTest.cs
```

## 2. Estratégia de Testes de Integração

### 2.1 Objetivo
Validar a persistência e recuperação de dados, garantindo que as queries Dapper e o mapeamento EF Core em `UserModel` estão corretos.

### 2.2 Infraestrutura
- **Banco de Dados**: SQLite In-Memory.
- **Dapper**: SQLite suporta a maioria das sintaxes ANSI SQL usadas no projeto.
- **Mapeamento**: Será necessário configurar o SQLite para usar o esquema das tabelas em tempo de execução.

### 2.3 Estrutura de Teste
```
NievoEasyfin.Tests/
├── Integration/
│   ├── Database/
│   │   ├── DatabaseTestBase.cs (Setup do SQLite + Migrations/Schema)
│   │   └── UserModelTest.cs
```

## 3. Plano de Ação

1. **Sprint 1: Refatoração para Interfaces**
   - Extrair interfaces para `CryptoPasswordService`, `JsonWebTokenService`, `AuthDbCacheService`, `UserModel`, `UserProviderSsoModel`, `SSoProviderAuth`, `SmtpModel`.
   - Atualizar `AuthenticatorService` e `Startup.cs`.

2. **Sprint 2: Testes Unitários do Service**
   - Criar `AuthenticatorServiceTest`.
   - Cobrir cenários: Login com sucesso, senha errada, usuário não encontrado, falha no JWT, etc.

3. **Sprint 3: Infraestrutura de Integração**
   - Criar `DatabaseTestBase` com SQLite.
   - Implementar testes para `UserModel`.
