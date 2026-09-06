# Fluxo de Autenticação e Segurança

A camada de autenticação e segurança do **Nievo EasyFin** é centralizada no microsserviço `NievoEasyFin.Auth`, gerenciando acessos ao schema `user_details` (dados e status) e `journey` (provedores SSO e termos).

---

## 🔑 1. Autenticação Local (Login Tradicional)

```mermaid
sequenceDiagram
    participant Frontend as Frontend (React / Vite)
    participant Auth as AuthService (C#)
    participant Crypto as CryptoPasswordService
    participant DB as Postgres (user_details.user)
    participant JWT as JsonWebTokenService

    Frontend->>Auth: POST /singin (email, password)
    Auth->>DB: GetUserByEmailAsync(email, statusId = 2)
    alt Usuário não encontrado ou status != 2 (ACTIVE)
        DB-->>Auth: Retorna null
        Auth-->>Frontend: HTTP 400 Bad Request (Credenciais inválidas / E-mail não validado)
    else Usuário ativo encontrado
        DB-->>Auth: Retorna UserEntity (com hash de senha)
        Auth->>Crypto: HashPasswordAsync(password) e compara
        alt Senha incorreta
            Crypto-->>Auth: Retorna false
            Auth-->>Frontend: HTTP 400 Bad Request (Credenciais inválidas)
        else Senha correta
            Crypto-->>Auth: Retorna true
            Auth->>JWT: GenerateToken(email, sub, exp, kid)
            JWT-->>Auth: Retorna string JWT Token
            Auth-->>Frontend: HTTP 200 OK (Token JWT + Expiração)
        end
    end
```

---

## 🌐 2. Autenticação SSO Google

```mermaid
sequenceDiagram
    participant Frontend as Frontend
    participant Auth as AuthService (C#)
    participant Google as Google OAuth2 API
    participant DB as Postgres (user_details & journey)

    Frontend->>Auth: POST /singin-sso (provider: "google", provider_access_token)
    Auth->>Google: ValidateProviderAsync (valida token e aud = GOOGLE_ID_CLIENT)
    Google-->>Auth: Retorna payload (email, sub, name)
    Auth->>DB: GetUserByProviderSubAndIdAsync (JOIN user_details.user + journey.user_provider_sso)
    alt Vínculo SSO já existe
        DB-->>Auth: Retorna UserEntity
    else Vínculo não existe
        Auth->>DB: CreateUserAsync (status ACTIVE = 2) + CreateUserProviderSsoEntityAsync
    end
    Auth-->>Frontend: Retorna Token JWT assinado (HS256)
```

---

## 🔄 3. Redefinição de Senha por PIN

1. **Solicitação (`POST /password-reset`):** O usuário informa o e-mail. Se existir na tabela `user_details.user`, é gerado um PIN de 6 dígitos salvo no Redis (chave `reset_password:{email}`, TTL de 15 min) e enviado via `SmtpProvider`.
2. **Confirmação (`PATCH /password-reset`):** O usuário envia e-mail, PIN e nova senha. O sistema valida o PIN no Redis, gera o novo hash PBKDF2 e atualiza a coluna `password` na tabela `user_details.user` via `UpdateUserPasswordAsync`.
