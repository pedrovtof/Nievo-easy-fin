# Endpoints de Contas, Cartões, Metas e Categorias (Core Service)

O Monólito `NievoEasyFin.Core` (Porta interna `8082`, rota de entrada Kong `/api/core`) gerencia as entidades do domínio financeiro, incluindo instituições bancárias, contas de usuário, cartões de crédito/débito, planejamento de metas e hierarquia de categorias.

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
  - `200 OK`: Cartão do usuário cadastrado (`accounts.user_bank_card`).
  - `404 Not Found`: Banco, Cartão ou Usuário não encontrado.

---

### 9. `POST /api/public/v1/Accounts/user:goal`
Cria uma nova meta financeira ou teto de orçamento para o usuário logado (`goals.goals`).

- **Headers:** `Authorization: Bearer <token_jwt>` (com claim `email`).
- **Corpo da Requisição:**
  ```json
  {
    "name": "Reserva de Emergência",
    "description": "Acumular 6 meses de despesas essenciais",
    "amount": 25000,
    "is_percent": false,
    "expire_at": "2027-12-31T00:00:00Z"
  }
  ```
- **Regras de Validação:**
  - `name`: Obrigatório, com tamanho entre 2 e 99 caracteres.
  - `amount`: Valor monetário ou percentual maior que zero (`amount > 0`).
  - `expire_at`: Obrigatório e data no futuro (`expire_at > DateTime.Today`).
  - **Unicidade:** Não é permitido duplicar o nome de uma meta ativa para o mesmo usuário.
- **Respostas:**
  - `200 OK`: Meta criada com sucesso (`EnumErrosApi.POSTUSERGOALASYNC_CORESERVICE_200_GOAL_CREATED`).
  - `400 Bad Request`: Parâmetros inválidos, data expirada ou meta já existente (`POSTUSERGOALASYNC_CORESERVICE_400_GOAL_ALREADY_EXIST`).
  - `404 Not Found`: Usuário não encontrado.

---

### 10. `GET /api/public/v1/Accounts/user:goal`
Retorna a lista paginada de metas financeiras do usuário logado.

- **Headers:** `Authorization: Bearer <token_jwt>`.
- **Parâmetros de Consulta (Query Params):**
  - `active` (`boolean`, opcional, padrão: `true`): Filtra por metas ativas ou inativas.
  - `page` (`int`, opcional, padrão: `1`): Número da página.
  - `page_size` (`int`, opcional, padrão: `10`): Quantidade de itens por página.
- **Respostas:**
  - `200 OK`: Retorna `ResponsePaginationBase<UserGoalView>` contendo lista de itens com `id`, `name`, `description`, `active`, `amount`, `is_percent`, `expire_at`, `created_at`, `updated_at`.
  - `400 Bad Request`: Paginação inválida.
  - `404 Not Found`: Usuário não encontrado.

---

### 11. `POST /api/public/v1/Accounts/user:category`
Cria uma categoria financeira associada à conta do usuário (`goals.category`).

- **Headers:** `Authorization: Bearer <token_jwt>` (com claim `email`).
- **Corpo da Requisição:**
  ```json
  {
    "name": "Supermercado",
    "description": "Compras de alimentos e mantimentos",
    "goal": 1,
    "parent_category": null
  }
  ```
- **Regras de Negócio e Validação:**
  - `name`: Obrigatório, tamanho entre 2 e 99 caracteres.
  - **Exclusividade Mútua Obrigatória:** A categoria **deve** ser associada estritamente a uma Meta (`goal > 0`) **OU** a uma Categoria Pai (`parent_category > 0`), nunca a ambos e nunca a nenhum (`POSTUSERCATEGORYASYNC_CORESERVICE_400_CATEGORY_ONLY_CAN_BE_CREATED_WITH_ONE_GOAL_OR_PARENT`).
  - Se informada `parent_category`, ela deve existir e estar ativa no banco (`POSTUSERCATEGORYASYNC_CORESERVICE_404_PARENTCATEGORY_NOT_FOUND`).
  - **Unicidade:** Não é permitido duplicar o nome de uma categoria ativa para o mesmo usuário (`POSTUSERCATEGORYASYNC_CORESERVICE_400_CATEGORY_ALREADY_EXIST`).
- **Respostas:**
  - `200 OK`: Categoria criada com sucesso (`EnumErrosApi.POSTUSERCATEGORYASYNC_CORESERVICE_200_CREATED`).
  - `400 Bad Request`: Violação de vínculo exclusivo, nome duplicado ou campos inválidos.
  - `404 Not Found`: Usuário ou Categoria Pai não encontrada.

---

### 12. `GET /api/public/v1/Accounts/user:category`
Retorna a lista paginada de categorias do usuário logado, incluindo o nome e descrição da meta vinculada via `LEFT JOIN`.

- **Headers:** `Authorization: Bearer <token_jwt>`.
- **Parâmetros de Consulta (Query Params):**
  - `active` (`boolean`, opcional, padrão: `true`): Filtra por categorias ativas ou inativas.
  - `page` (`int`, opcional, padrão: `1`): Número da página.
  - `page_size` (`int`, opcional, padrão: `10`): Quantidade de itens por página.
- **Respostas:**
  - `200 OK`: Retorna `ResponsePaginationBase<UserCategoryView>` contendo itens com `id`, `name`, `description`, `active`, `parent_category`, `goal_name`, `goal_description`, `created_at`, `updated_at`.
  - `400 Bad Request`: Paginação inválida.
  - `404 Not Found`: Usuário não encontrado.

---

## 🔒 Endpoints Privados / Administrativos (`/api/private/v1/Accounts`)

Reservados a operações administrativas do sistema para expansão do catálogo de bancos e cartões.

### 13. `POST /api/private/v1/Accounts/banks`
Cadastra uma nova instituição financeira no sistema global.

- **Corpo da Requisição:**
  ```json
  {
    "name": "Nubank",
    "bank_type": 1
  }
  ```
- **Respostas:**
  - `200 OK`: Banco criado com sucesso (`accounts.bank`).
  - `400 Bad Request`: Banco já existente ou tipo inválido.

---

### 14. `POST /api/private/v1/Accounts/bank-card`
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
  - `200 OK`: Cartão cadastrado no catálogo (`accounts.bank_card`).
  - `404 Not Found`: Banco, Tipo de cartão ou Bandeira não encontrada.

---

## 🛠️ Endpoints de Infraestrutura (`/api/admin/v1/...`)

### 15. `GET /api/admin/v1/HealthCheck`
Verifica a saúde e conectividade do monólito Core.

- **Respostas:**
  - `200 OK`: Status do serviço e conexões com o PostgreSQL e Redis.
