# Fluxo de Gestão de Contas Bancárias e Cartões

A gestão de contas e cartões no **Nievo EasyFin** é coordenada pelo Monólito `NievoEasyFin.Core` atuando sobre o esquema **`accounts`** do PostgreSQL, estruturada em controladores RESTful específicos (`UserController`, `BankController` e `CardController`).

---

## 🏦 1. Vínculo de Contas Bancárias (`accounts.user_bank`)

```mermaid
flowchart TD
    User["Usuário no Frontend"] --> SearchBank["Consulta Lista de Bancos (GET /api/public/v1/Bank)"]
    SearchBank --> SelectBank["Seleciona o Banco (accounts.bank) e digita Apelido"]
    SelectBank --> PostUserBank["POST /api/public/v1/User/banks (Bearer JWT)"]
    
    PostUserBank --> CheckAuth{"Token JWT Válido?"}
    CheckAuth -->|Não| HTTP401["HTTP 401 Unauthorized"]
    CheckAuth -->|Sim| FindUser["Obtém E-mail da Claim e busca user_details.user"]

    FindUser --> FindBank{"Banco e Tipo de Banco existem no Redis / accounts.bank?"}
    FindBank -->|Não| HTTP404["HTTP 404 Not Found (Banco não encontrado)"]

    FindBank -->|Sim| CheckExists{"Vínculo (user_id, bank_id) já existe?"}
    CheckExists -->|Sim| HTTP400["HTTP 400 Bad Request (Conta bancária já vinculada)"]

    CheckExists -->|Não| SaveUserBank["Cria registro em accounts.user_bank (CreateUserBankAsync)"]
    SaveUserBank --> HTTP200["HTTP 200 OK (Conta bancária vinculada com sucesso)"]
```

---

## 💳 2. Cadastro e Vínculo de Cartões (`accounts.user_bank_card`)

### 2.1 Fluxo de Decisão (Decision Flow - `POST /api/public/v1/User/bank-card`)

```mermaid
flowchart TD
    start_node(["([Início: Usuário solicita vínculo de cartão])"]) --> input_data[/Recebe POST /api/public/v1/User/bank-card com Token JWT/]
    input_data --> validate_dto{Payload válido no PostUserCardValidatorAsync?}
    validate_dto -->|Não| err_400_dto(["([Fim: HTTP 400 Bad Request - Erro de Validação])"])
    validate_dto -->|Sim| check_bank{Banco existe e está ativo em accounts.bank?}

    check_bank -->|Não| err_404_bank(["([Fim: HTTP 404 Not Found - BANK_NOT_FOUND])"])
    check_bank -->|Sim| check_card{Cartão pertence ao Banco e está ativo em accounts.bank_card?}

    check_card -->|Não| err_404_card(["([Fim: HTTP 404 Not Found - BANKCARD_NOT_FOUND])"])
    check_card -->|Sim| check_user{Usuário existe em user_details.user?}

    check_user -->|Não| err_404_user(["([Fim: HTTP 404 Not Found - USER_NOT_FOUND])"])
    check_user -->|Sim| check_unique{Já existe cartão ativo para card_id + user_id?}

    check_unique -->|Sim| err_400_exists(["([Fim: HTTP 400 Bad Request - USER_CARD_ALREADY_EXISTS])"])
    check_unique -->|Não| persist_card[(Insere registro em accounts.user_bank_card)]

    persist_card --> check_created{Registro persistido com sucesso?}
    check_created -->|Não| err_not_created(["([Fim: Erro ao criar cartão - CARD_NOT_CREATED])"])
    check_created -->|Sim| end_ok(["([Fim: HTTP 200 OK - CARD_CREATED])"])
```

### 2.2 Casos de Uso (Use Case Diagram - Gestão de Contas e Cartões)

```mermaid
flowchart LR
    actor_user["👤 Usuário Autenticado"]
    actor_admin["👤 Administrador"]

    subgraph CoreAccountsSystem ["Sistema: Nievo EasyFin Core (Módulo Accounts)"]
        uc_list_banks(["Consultar Catálogo de Bancos"])
        uc_link_bank(["Vincular Conta Bancária"])
        uc_list_user_banks(["Listar Contas do Usuário"])
        uc_list_bank_cards(["Consultar Cartões do Banco"])
        uc_link_user_card(["Vincular Cartão à Carteira do Usuário"])
        uc_validate_unique_card(["Validar Unicidade Cartão + Usuário (idx_unic_user_bank_card)"])
        uc_list_user_cards(["Listar Cartões da Carteira"])
        uc_create_catalog_card(["Cadastrar Cartão no Catálogo Global"])

        uc_link_user_card -.->|"<<include>>"| uc_validate_unique_card
        uc_link_user_card -.->|"<<include>>"| uc_list_bank_cards
    end

    actor_user --- uc_list_banks
    actor_user --- uc_link_bank
    actor_user --- uc_list_user_banks
    actor_user --- uc_list_bank_cards
    actor_user --- uc_link_user_card
    actor_user --- uc_list_user_cards
    actor_admin --- uc_create_catalog_card
```

### 2.3 Diagrama de Classes (`CoreUserService` e `UserBankCardModel`)

```mermaid
classDiagram
    direction TB

    class UserController {
        <<controller>>
        -ICoreUserService _userService
        -JsonWebTokenService _jsonWebTokenService
        +PostUserBanks(authorization: string, request: PostUserBanksRequest) Task~IActionResult~
        +GetUserBanks(authorization: string, request: GetUserBanksRequest) Task~IActionResult~
        +GetUserCard(authorization: string, request: GetUserCardRequest) Task~IActionResult~
        +PostUserCard(authorization: string, request: PostUserCardRequest) Task~IActionResult~
    }

    class ICoreUserService {
        <<interface>>
        +PostUserBanks(request: PostUserBanksRequest) Task~IActionResult~
        +GetUserBanks(request: GetUserBanksRequest) Task~IActionResult~
        +GetUserCard(request: GetUserCardRequest) Task~IActionResult~
        +PostUserCard(request: PostUserCardRequest) Task~IActionResult~
        +PostUserGoal(request: PostUserGoalRequest) Task~IActionResult~
        +GetUserGoal(request: GetUserGoalRequest) Task~IActionResult~
        +PostUserCategory(request: PostUserCategoryRequest) Task~IActionResult~
        +GetUserCategory(request: GetUserCategoryRequest) Task~IActionResult~
    }

    class CoreUserService {
        <<service>>
        -UserModel _userModel
        -UserBankModel _userBankModel
        -UserBankCardModel _userBankCardModel
        -BankModel _bankModel
        -BankCardModel _bankCardModel
        +PostUserCard(request: PostUserCardRequest) Task~IActionResult~
        +GetUserCard(request: GetUserCardRequest) Task~IActionResult~
    }

    class UserBankCardModel {
        <<repository>>
        -CoreOrigin _CoreMainNodeDatabase
        -CoreReplica _CoreReplicaNodeDatabase
        +CreateUserBankCard(bankId: int, cardId: int, userId: int, cardUserName: string, expireAt: DateTime) Task~UserBankCardEntity~
        +GetUserBankCardByCardIdAndUserId(card_id: int, user_id: int) Task~UserBankCardEntity~
        +GetUserBankCard(page: int, pageSize: int, bankId: int?, userId: int, active: bool, flag: string?) Task~Tuple~
    }

    class UserBankCardEntity {
        <<entity>>
        +int Id
        +int BankId
        +string Name
        +int CardId
        +bool Active
        +int UserId
        +DateTime ExpiredAt
        +DateTime CreatedAt
        +DateTime UpdatedAt
    }

    class PostUserCardRequest {
        +int BankId
        +string CardUserName
        +int CardId
        +DateTime ExpireAt
    }

    class PostUserCardValidatorAsync {
        +ValidateAsync(request: PostUserCardRequest) Task~ValidationResult~
        -ValidateExpiredAt(x: DateTime) bool
    }

    UserController --> ICoreUserService : delegates
    CoreUserService ..|> ICoreUserService : implements
    CoreUserService --> UserBankCardModel : uses
    CoreUserService ..> PostUserCardRequest : receives
    CoreUserService ..> PostUserCardValidatorAsync : validates with
    UserBankCardEntity <|-- UserBankCardModel : inherits
```

### 📌 Tabelas e Índices do Esquema `accounts` Utilizados:
- `accounts.bank`: Instituições bancárias cadastradas (`id`, `name`, `bank_type`, `active`).
- `accounts.bank_type`: Classificação de bancos (`id`, `name`, `description`).
- `accounts.user_bank`: Vínculo entre usuário e banco (`id`, `user_id`, `bank_id`, `nick_name`, `active`).
- `accounts.bank_card`: Catálogo de cartões (`id`, `bank_id`, `name`, `card_type`, `flag_id`, `active`).
- `accounts.bank_card_type`: Tipos de cartão (`Crédito`, `Débito`, `Múltiplo`).
- `accounts.bank_card_flag`: Bandeiras (`Visa`, `Mastercard`, `Elo`, `Amex`).
- `accounts.user_bank_card`: Cartões do usuário (`id`, `bank_id`, `card_id`, `user_id`, `name`, `expired_at`, `active`), protegido pelo índice único `idx_unic_user_bank_card` sobre `(card_id, user_id)`.

---

## 🛡️ 3. Operações Administrativas (`/api/private/v1/Bank`)

- **`POST /api/private/v1/Bank`:** Insere registros de instituições financeiras na tabela `accounts.bank`.
- **`POST /api/private/v1/Bank/card`:** Insere novos produtos no catálogo de cartões `accounts.bank_card`.
