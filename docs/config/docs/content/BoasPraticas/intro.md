# Boas Práticas e Padrões Técnicos

Para garantir a manutenibilidade, testabilidade, legibilidade e segurança do código-fonte no **Nievo EasyFin**, todo o desenvolvimento deve seguir rigorosamente as diretrizes e padrões técnicos descritos neste documento.

---

## 🏛️ 1. Padrões de Arquitetura Backend (C# / .NET)

### 🧩 Separação em Camadas e Injeção de Dependências
- **Interfaces (`NievoEasyFin.Application/Interfaces`):** Todos os serviços, repositórios e clientes externos devem ser expostos via interfaces. É expressamente proibido injetar implementações concretas diretamente nos controllers.
- **DTOs de Requisição e Resposta:** Controllers devem aceitar estritamente objetos Request DTO (`PostCreateUserRequest`, `PostUserBanksRequest`) e retornar DTOs de resposta (`ResponseApiSucess`, `ResponseApiError`). Entidades EF Core do banco de dados nunca devem ser expostas na API.
- **FluentValidation:** Todas as validações de DTOs de entrada devem ser isoladas em validadores assíncronos que implementam `AbstractValidator<T>` (ex: `PostCreateUserValidator`, `PostUserBanksValidatorAsync`).

---

## 🔐 2. Segurança e Tratamento de Dados Sensíveis

- **Senhas do Usuário:** Nunca armazenar senhas em texto puro ou criptografia reversível. Utilizar exclusivamente o componente `CryptoPasswordService` com hashing **PBKDF2 / HMAC-SHA256**.
- **Chaves de Assinatura JWT:** O segredo de assinatura do JWT nunca deve estar exposto em código. Deve ser lido de variáveis de ambiente via `DotNetEnv.Env.GetString("JWT_SECRET")`.
- **Validação de Claims:** Métodos protegidos que exigem identificação do usuário devem extrair as informações diretamente do token JWT utilizando `JsonWebTokenService.GetClaimValue(authorization, "email")`.

---

## 💻 3. Padrões do Frontend (React / Vite / MUI)

- **Estrutura Modular por Componente:** Manter a separação de responsabilidades no padrão de 4 arquivos (`index.jsx`, `View.jsx`, `styles.js`, `api.js`).
- **Hooks Personalizados:** Encapsular lógicas reutilizáveis em Hooks React (ex: `useText` para internacionalização e mensagens de UI).
- **Consistência de Estilo:** Utilizar componentes da biblioteca MUI (`@mui/material`) e estilizá-los prioritariamente via props `sx` ou `styled()` do MUI, evitando CSS global disperso.

---

## 🧪 4. Padrões de Testes Unitários

- **Padrão AAA (Arrange, Act, Assert):** Todo teste unitário deve estar estruturado em três blocos claramente demarcados:
  - `Arrange`: Preparação de dados de entrada e Mocks de dependências.
  - `Act`: Invocação do método sob teste.
  - `Assert`: Validação dos resultados e asserção dos retornos HTTP ou exceções.
- **Convenção de Nomenclatura dos Métodos de Teste:**
  `NomeDoMetodo_Cenario_ResultadoEsperado`  
  *Exemplo:* `PostCreateUserAsync_WithValidRequest_ReturnsCreated()`

---

## 🗄️ 5. Convenções de Banco de Dados

- **Nomenclatura em Snake Case:** Tabelas e colunas no PostgreSQL devem utilizar stritamente `snake_case` (ex: `user_provider_sso`, `accept_terms_id`).
- **Organização por Schemas:** Entidades de identidade e login ficam no schema `journey`; entidades bancárias e cartões ficam no schema `bank`.
- **Migrações Exclusivas:** Nenhuma alteração manual deve ser feita no banco de dados de desenvolvimento ou produção; todas as mudanças devem ser versionadas em scripts do **Alembic** ou **EF Core Migrations**.