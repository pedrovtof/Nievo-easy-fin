# Endpoints de Contas e Cartões (Core Service)

O Monólito `NievoEasyFin.Core` (Porta interna `8082`, rota de entrada Kong `/api/core`) gerencia as entidades do domínio financeiro, incluindo instituições bancárias, contas de usuário e cartões de crédito/débito.

---

## 🟢 Endpoints Públicos / Protegidos por Usuário (`/api/public/v1/Accounts`)

Estes endpoints exigem o cabeçalho `Authorization: Bearer <token_jwt>` contendo a claim do e-mail do usuário autenticado.

### 1. `GET /api/public/v1/Accounts/banks`
Retorna uma lista paginada de instituições financeiras cadastradas no sistema.

- **Parâmetros de Consulta (Query Params):**
  - `page`: Número da página (padrão: 1).
  - `page_size`: Quantidade de itens por página (padrão: 10).
- **Respostas:**
  - `200 OK`: Retorna `ResponsePaginationBase<GetBanksResponse>`.

---

### 2. `POST /api/public/v1/Accounts/user-banks`
Vincula uma conta bancária ao usuário logado.

- **Corpo da Requisição:**
  ```json
  {
    "bank_name": "Itaú Unibanco",
    "bank_type": 1,
    "nick_name": "Minha Conta Principal"
  }
  ```
- **Respostas:**
  - `200 OK`: Vínculo criado com sucesso (`bank.user_banks`).
  - `400 Bad Request`: Vínculo já cadastrado para este usuário.
  - `404 Not Found`: Usuário ou Banco não encontrado.

---

### 3. `GET /api/public/v1/Accounts/user-banks`
Retorna a lista de contas bancárias cadastradas pelo usuário logado.

- **Parâmetros de Consulta:** `page`, `page_size`.
- **Respostas:**
  - `200 OK`: Lista de objetos `GetUserBanksResponse`.

---

### 4. `GET /api/public/v1/Accounts/card-type`
Retorna os tipos de cartão disponíveis (`Crédito`, `Débito`, `Múltiplo`).

- **Parâmetros de Consulta:** `page`, `page_size`.
- **Respostas:**
  - `200 OK`: Lista paginada `GetCardTypeResponse`.

---

### 5. `GET /api/public/v1/Accounts/card-flag`
Retorna as bandeiras de cartão cadastradas (`Visa`, `Mastercard`, `Elo`, `Amex`).

- **Parâmetros de Consulta:** `page`, `page_size`.
- **Respostas:**
  - `200 OK`: Lista paginada `GetCardFlagesponse`.

---

### 6. `GET /api/public/v1/Accounts/bank-card`
Consulta o catálogo de cartões de banco filtrado por parâmetros.

- **Parâmetros de Consulta:** `page`, `page_size`, `bank_id`, `card_type`, `flag`.
- **Respostas:**
  - `200 OK`: Lista paginada de `BankCardView`.

---

### 7. `GET /api/public/v1/Accounts/user:bank-card`
Retorna os cartões bancários cadastrados pelo usuário logado.

- **Parâmetros de Consulta:** `page`, `page_size`, `bank_id`, `active`, `flag`.
- **Respostas:**
  - `200 OK`: Lista paginada `GetUserBankCardResponse`.

---

### 8. `POST /api/public/v1/Accounts/user:bank-card`
Cadastra um cartão de banco na carteira do usuário logado.

- **Corpo da Requisição:**
  ```json
  {
    "bank_id": 2,
    "card_id": 5,
    "card_user_name": "Cartão Black Itaú",
    "expire_at": "2029-12-31T23:59:59Z"
  }
  ```
- **Respostas:**
  - `200 OK`: Cartão do usuário cadastrado (`bank.user_bank_cards`).
  - `404 Not Found`: Banco, Cartão ou Usuário não encontrado.

---

## 🔒 Endpoints Privados / Administrativos (`/api/private/v1/Accounts`)

Reservados a operações administrativas do sistema para expansão do catálogo de bancos e cartões.

### 9. `POST /api/private/v1/Accounts/banks`
Cadastra uma nova instituição financeira no sistema global.

- **Corpo da Requisição:**
  ```json
  {
    "name": "Nubank",
    "bank_type": 1
  }
  ```
- **Respostas:**
  - `200 OK`: Banco criado com sucesso (`bank.banks`).
  - `400 Bad Request`: Banco já existente ou tipo inválido.

---

### 10. `POST /api/private/v1/Accounts/bank-card`
Cadastra um novo produto de cartão no catálogo global do sistema.

- **Corpo da Requisição:**
  ```json
  {
    "bank_id": 2,
    "card_type": 1,
    "name": "Itaú Personnalité Black",
    "flag": "Mastercard"
  }
  ```
- **Respostas:**
  - `200 OK`: Cartão cadastrado no catálogo (`bank.bank_cards`).
  - `404 Not Found`: Banco, Tipo de cartão ou Bandeira não encontrada.

---

## 🛠️ Endpoints de Infraestrutura (`/api/admin/v1/...`)

### 11. `GET /api/admin/v1/HealthCheck`
Verifica a saúde e conectividade do monólito Core.

- **Respostas:**
  - `200 OK`: Status do serviço e conexões com o PostgreSQL e Redis.
