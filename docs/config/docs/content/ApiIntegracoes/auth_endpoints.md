# Endpoints de Autenticação (Auth Service)

O microsserviço `NievoEasyFin.Auth` (Porta interna `8081`, rota de entrada Kong `/api/auth`) gerencia todo o ciclo de vida da identidade do usuário, tokens JWT, termos de uso e recuperação de credenciais.

Em conformidade com os princípios **SOLID** (Princípio da Responsabilidade Única - *SRP* e Princípio da Segregação de Interfaces - *ISP*) e padrões **RESTful**, as responsabilidades foram organizadas entre:
- **`UsersController`** (`/api/public/v1/Users`): Consome `IUsersService` para criação de usuários (tradicional e SSO) e fluxo de redefinição de senha (`password-reset`).
- **`AuthenticatorController`** (`/api/public/v1/Authenticator`): Consome `IAuthenticatorService` para operações de autenticação de sessão (`singin`), validação de e-mail por PIN e consulta aos termos de uso vigentes.
- **`HealthCheckController`** (`/api/admin/v1/HealthCheck`): Endpoints administrativos de verificação de nós de banco de dados, cache Redis e serviço SMTP.

---

## 👤 1. Endpoints de Usuários e Recuperação (`/api/public/v1/Users`)

### 1. `POST /api/public/v1/Users/singup`
Cria uma nova conta de usuário tradicional por e-mail e senha.

- **Headers Obrigatórios:**
  - `User-Agent`: Identificação do cliente.
  - `Host`: Endereço IP / Host de origem.
- **Corpo da Requisição (Request Body):**
  ```json
  {
    "name": "Joe Black",
    "email": "joe.black@example.com",
    "password": "1Meet-Death!",
    "accept_terms": true
  }
  ```
- **Respostas:**
  - `201 Created`: Usuário criado com sucesso (Status: `INVALID` / Aguardando confirmação por PIN).
    ```json
    {
      "sucess": true,
      "data": "Usuário criado com sucesso. Verifique seu e-mail."
    }
    ```
  - `400 Bad Request`: Erro de validação de campos, senha fraca, e-mail já existente ou recusa dos termos (`accept_terms: false`).
  - `409 Conflict`: E-mail já cadastrado e ativado no sistema.

---

### 2. `POST /api/public/v1/Users/singup-sso`
Cria ou vincula uma conta de usuário utilizando login social (Google OAuth).

- **Headers Obrigatórios:** `User-Agent`, `Host`.
- **Corpo da Requisição:**
  ```json
  {
    "provider_name": "google",
    "provider_access_token": "ya29.a0ARW5m76...",
    "accept_terms": true
  }
  ```
- **Respostas:**
  - `201 Created`: Novo usuário criado via SSO com status `ACTIVE` e vínculo salvo em `journey.user_provider_sso`.
  - `200 OK`: Usuário SSO já existente no sistema.
  - `400 Bad Request`: Token do provedor inválido, provedor inativo ou termos não aceitos.

---

### 3. `POST /api/public/v1/Users/password-reset`
Inicia o processo de recuperação de conta solicitando um PIN numérico de 6 dígitos para o e-mail do usuário.

- **Corpo da Requisição:**
  ```json
  {
    "email": "joe.black@example.com"
  }
  ```
- **Respostas:**
  - `200 OK`: PIN gerado e armazenado em cache Redis (`reset_password:{email}`), enviado por e-mail via SMTP.
  - `400 Bad Request`: E-mail inválido ou usuário não localizado.

---

### 4. `PATCH /api/public/v1/Users/password-reset`
Conclui a redefinição de senha informando o PIN recebido por e-mail e a nova senha.

- **Corpo da Requisição:**
  ```json
  {
    "email": "joe.black@example.com",
    "pin_token": "482910",
    "password": "NewStrongPassword123!"
  }
  ```
- **Respostas:**
  - `200 OK`: Senha atualizada com sucesso no PostgreSQL (`user_details.user`).
  - `400 Bad Request`: PIN inválido, expirado ou senha que não atende aos requisitos de segurança.

---

## 🔑 2. Endpoints de Sessão e Autenticação (`/api/public/v1/Authenticator`)

### 5. `POST /api/public/v1/Authenticator/singin`
Realiza a autenticação de um usuário tradicional (e-mail e senha).

- **Corpo da Requisição:**
  ```json
  {
    "email": "joe.black@example.com",
    "password": "1Meet-Death!"
  }
  ```
- **Respostas:**
  - `200 OK`: Retorna o Token JWT assinado para autorização nas chamadas subsequentes.
    ```json
    {
      "sucess": true,
      "data": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "expires_in": 86400
      }
    }
    ```
  - `400 Bad Request`: Credenciais incorretas ou e-mail pendente de validação.

---

### 6. `POST /api/public/v1/Authenticator/singin-sso`
Realiza o login de usuário via SSO Google.

- **Corpo da Requisição:**
  ```json
  {
    "provider_name": "google",
    "provider_access_token": "ya29.a0ARW5m76..."
  }
  ```
- **Respostas:**
  - `200 OK`: Retorna o Token JWT.
  - `400 Bad Request`: Falha na autenticação junto ao provedor.

---

### 7. `POST /api/public/v1/Authenticator/validate:email`
Confirma a ativação da conta do usuário informando o PIN de 6 dígitos enviado por e-mail no cadastro.

- **Corpo da Requisição:**
  ```json
  {
    "email": "joe.black@example.com",
    "pin_token": "123456"
  }
  ```
- **Respostas:**
  - `200 OK`: Conta ativada com sucesso (`status_id = 2`).
  - `400 Bad Request`: PIN incorreto ou expirado.

---

### 8. `POST /api/public/v1/Authenticator/send-validate:email`
Solicita o reenvio do e-mail de ativação de conta com um novo PIN temporário.

- **Corpo da Requisição:**
  ```json
  {
    "email": "joe.black@example.com"
  }
  ```
- **Respostas:**
  - `200 OK`: E-mail de validação reenviado.
  - `400 Bad Request`: E-mail não localizado.

---

### 9. `GET /api/public/v1/Authenticator/accept-terms:singup`
Retorna o Termo de Uso ativo vigente para exibição na tela de cadastro.

- **Respostas:**
  - `200 OK`: Retorna os dados do termo (`title`, `description`, `content`, `version`, `code`).

---

## 🛠️ 3. Endpoints Administrativos (`/api/admin/v1/HealthCheck`)

### 10. `GET /api/admin/v1/HealthCheck`
Verifica a saúde básica do microsserviço de autenticação.

- **Autorização:** Livre (`[AllowAnonymous]`).
- **Respostas:**
  - `200 OK`: Retorna `{"message": "Alive"}`.

---

### 11. `GET /api/admin/v1/HealthCheck/pgsql:main-node`
Valida conectividade com o nó Master do PostgreSQL (`AuthOrigin`).

- **Headers:** `Authorization: Bearer <token_jwt>`
- **Respostas:**
  - `200 OK`: Retorna `{"test_mainNode": 1}`.
  - `400 Bad Request`: Falha na conexão com nó principal.

---

### 12. `GET /api/admin/v1/HealthCheck/pgsql:replica-node`
Valida conectividade com o nó Réplica do PostgreSQL (`AuthReplica`).

- **Headers:** `Authorization: Bearer <token_jwt>`
- **Respostas:**
  - `200 OK`: Retorna `{"test_replicaNode": 1}`.
  - `400 Bad Request`: Falha na conexão com réplica de leitura.

---

### 13. `GET /api/admin/v1/HealthCheck/cache:main-node`
Valida gravação e leitura no cache Redis (`AuthDbCacheService`).

- **Headers:** `Authorization: Bearer <token_jwt>`
- **Respostas:**
  - `200 OK`: Cache operacional.
  - `400 Bad Request`: Falha de comunicação com o Redis.

---

### 14. `POST /api/admin/v1/HealthCheck/smtp`
Dispara teste de envio de e-mail através do serviço SMTP configurado (`SmtpModel`).

- **Headers:** `Authorization: Bearer <token_jwt>`
- **Corpo da Requisição:** `"Joe.Black@example.com"`
- **Respostas:**
  - `200 OK`: Requisição de disparo enviada.
