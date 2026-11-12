# Fluxo de Transações Financeiras e Situação de Caixa (`payment`)

O gerenciamento de lançamentos financeiros e fluxo de caixa no **Nievo EasyFin** é coordenado pelo Monólito `NievoEasyFin.Core` através do controlador RESTful `PaymentController` (`/api/public/v1/Payment`) e do serviço `PaymentService`, operando sobre o esquema **`payment`** do PostgreSQL.

---

## 💸 1. Situação da Transação (`payment.transaction_situation`)

Toda transação financeira registrada na plataforma é classificada de acordo com sua situação de fluxo de caixa na tabela de domínio `payment.transaction_situation` e mapeada no backend pelo enumerador `EnumTransactionSituation`:
- **`Entrada` (`EnumTransactionSituation.ENTRADA = 1`)**: Receitas, créditos, salários, reembolsos ou aportes recebidos pelo usuário (mantém `amount` positivo).
- **`Saida` (`EnumTransactionSituation.SAIDA = 2`)**: Despesas, débitos, pagamentos de faturas ou transferências enviadas pelo usuário (converte automaticamente `request.Amount = request.Amount * -1` no `PaymentService` antes da gravação).

### 1.1 Fluxo de Decisão — Consulta de Situações (`GET /api/public/v1/Payment/situation`)

```mermaid
flowchart TD
    start_sit(["([Início: Cliente solicita situações de transação])"]) --> input_sit[/Recebe GET /api/public/v1/Payment/situation com page, page_size e active/]
    input_sit --> val_sit{Parâmetros válidos no GetPaymentTransactionSituationValidatorAsync?<br/>page > 0 e page_size entre 1..100}
    val_sit -->|Não| err_400_sit(["([Fim: HTTP 400 Bad Request - INVALID_REQUEST])"])
    val_sit -->|Sim| query_sit[(Consulta CoreReplica via TransactionSituationModel.GetTransactionSituationAsync)]
    query_sit --> check_records{Total de registros > 0?}
    check_records -->|Não| err_404_sit(["([Fim: HTTP 404 Not Found - SITUATIONS_NOT_FOUND])"])
    check_records -->|Sim| map_sit[Mapeia TransactionSituationView para GetPaymentTransactionSituationAsyncResponse]
    map_sit --> ok_200_sit(["([Fim: HTTP 200 OK - ResponsePaginationBase])"])
```

---

## 🧾 2. Registro e Consulta de Transações (`payment.transaction`)

### 2.1 Fluxo de Decisão — Criação de Transação (`POST /api/public/v1/Payment`)

```mermaid
flowchart TD
    start_tx(["([Início: Usuário registra nova transação])"]) --> input_tx[/Recebe POST /api/public/v1/Payment com Token JWT + PostPaymentTransactionRequest/]
    input_tx --> val_dto{"Payload válido no PostPaymentTransactionValidatorAsync?<br/>Name/Desc/Category não vazios, Amount > 0,<br/>Status > 0, Type > 0, Situation >= 1, Bank >= 0, Card >= 0, Scheduler >= 0"}
    val_dto -->|Não| err_400_tx(["([Fim: HTTP 400 Bad Request - Erro de Validação])"])
    val_dto -->|Sim| check_user{Usuário existe em user_details.user?}

    check_user -->|Não| err_404_user(["([Fim: HTTP 404 Not Found - USER_NOT_FOUND])"])
    check_user -->|Sim| check_cat{Categoria pertence ao usuário em goals.category?}

    check_cat -->|Não| err_404_cat(["([Fim: HTTP 404 Not Found - CATEGORY_NOT_FOUND])"])
    check_cat -->|Sim| check_status{Status existe e está ativo em payment.transaction_status?}

    check_status -->|Não| err_404_status(["([Fim: HTTP 404 Not Found - TRANSACTION_STATUS_NOT_FOUND])"])
    check_status -->|Sim| check_type{Tipo existe e está ativo em payment.transaction_type?}

    check_type -->|Não| err_404_type(["([Fim: HTTP 404 Not Found - TRANSACTION_TYPE_NOT_FOUND])"])
    check_type -->|Sim| check_situation{Situação existe e está ativa em payment.transaction_situation?}

    check_situation -->|Não| err_404_sit(["([Fim: HTTP 404 Not Found - SITUATION_NOT_FOUND])"])
    check_situation -->|Sim| check_payment_source{Banco e Cartão/Scheduler opcionais existem quando > 0?}

    check_payment_source -->|Não| err_404_source(["([Fim: HTTP 404 Not Found - BANK / CARD / SCHEDULER_NOT_FOUND])"])
    check_payment_source -->|Sim| check_is_saida{"situation.Name == EnumTransactionSituation.SAIDA ('Saida')?"}

    check_is_saida -->|Sim| negate_amount["Inverte sinal contábil: request.Amount = request.Amount * -1"]
    check_is_saida -->|"Não (Entrada)"| insert_tx[(Persiste em payment.transaction via TransactionModel.CreatePaymentTransaction)]
    negate_amount --> insert_tx

    insert_tx --> end_201_tx(["([Fim: HTTP 201 Created - POSTPAYMENTTRANSACTIONASYNC_CORESERVICE_201_SUCESS])"])
```

---

## 🎯 3. Casos de Uso (Use Case Diagram — Módulo Payment)

```mermaid
flowchart LR
    actor_user["👤 Usuário Autenticado"]

    subgraph CorePaymentSystem ["Sistema: Nievo EasyFin Core (Módulo Payment)"]
        uc_list_situations(["Consultar Situações de Transação (Entrada / Saída)"])
        uc_list_metadata(["Consultar Status, Tipos e Recorrências"])
        uc_list_transactions(["Listar Transações Paginadas"])
        uc_filter_situation(["Filtrar Transações por Situação (transaction_situation_id)"])
        uc_create_transaction(["Registrar Nova Transação Financeira"])
        uc_validate_situation(["Validar Situação da Transação (payment.transaction_situation)"])
        uc_apply_sign(["Aplicar Sinal Contábil por Situação (+ Entrada / - Saída)"])

        uc_list_transactions -.->|"<<extend>>"| uc_filter_situation
        uc_create_transaction -.->|"<<include>>"| uc_validate_situation
        uc_create_transaction -.->|"<<include>>"| uc_apply_sign
        uc_create_transaction -.->|"<<include>>"| uc_list_situations
        uc_create_transaction -.->|"<<include>>"| uc_list_metadata
    end

    actor_user --- uc_list_situations
    actor_user --- uc_list_metadata
    actor_user --- uc_list_transactions
    actor_user --- uc_create_transaction
```

---

## 🏗️ 4. Diagrama de Classes (`PaymentController`, `PaymentService` e `Models`)

```mermaid
classDiagram
    direction TB

    class PaymentController {
        <<controller>>
        -IPaymentService _paymentService
        -JsonWebTokenService _jsonWebTokenService
        +PostPaymentTransaction(authorization: string, request: PostPaymentTransactionRequest) Task~IActionResult~
        +GetPaymentTransaction(authorization: string, request: GetPaymentTransactionRequest) Task~IActionResult~
        +GetPaymentTransactionStatus(authorization: string, request: GetPaymentTransactionStatusRequest) Task~IActionResult~
        +GetPaymentTransactionType(authorization: string, request: GetPaymentTransactionTypeRequest) Task~IActionResult~
        +GetPaymentTransactionScheduler(authorization: string, request: GetPaymentTransactionSchedulerRequest) Task~IActionResult~
        +GetPaymentTransactionSituation(authorization: string, request: GetPaymentTransactionSituationRequest) Task~IActionResult~
    }

    class IPaymentService {
        <<interface>>
        +PostPaymentTransaction(request: PostPaymentTransactionRequest) Task~IActionResult~
        +GetPaymentTransaction(request: GetPaymentTransactionRequest) Task~IActionResult~
        +GetPaymentTransactionStatus(request: GetPaymentTransactionStatusRequest) Task~IActionResult~
        +GetPaymentTransactionType(request: GetPaymentTransactionTypeRequest) Task~IActionResult~
        +GetPaymentTransactionScheduler(request: GetPaymentTransactionSchedulerRequest) Task~IActionResult~
        +GetPaymentTransactionSituation(request: GetPaymentTransactionSituationRequest) Task~IActionResult~
    }

    class PaymentService {
        <<service>>
        -UserModel _userModel
        -CategoryModel _categoryModel
        -UserBankModel _userBankModel
        -UserBankCardModel _userBankCardModel
        -TransactionStatusModel _transactionStatusModel
        -TransactionTypeModel _transactionTypeModel
        -SchedulerTypeModel _schedulerTypeModel
        -TransactionModel _transactionModel
        -TransactionSituationModel _transactionSituationModel
        +PostPaymentTransaction(request: PostPaymentTransactionRequest) Task~IActionResult~
        +GetPaymentTransaction(request: GetPaymentTransactionRequest) Task~IActionResult~
        +GetPaymentTransactionSituation(request: GetPaymentTransactionSituationRequest) Task~IActionResult~
    }

    class TransactionModel {
        <<repository>>
        -CoreOrigin _coreOrigin
        -CoreReplica _coreReplica
        +CreatePaymentTransaction(name: string, description: string, userId: int, transactionStatusId: int, transactionTypeId: int, transactionSituationId: int, categorieId: int, payDate: DateTime, amount: long, cardId: int?, bankId: int?, schedulerId: int?) Task~TransactionEntity?~
        +GetPaymentTransactionAsync(userId: int, page: int, pageSize: int, transactionStatusId: int?, transactionTypeId: int?, transactionSituationId: int?, categorieId: int?, payDate: DateTime?, cardId: int?, bankId: int?, schedulerId: int?, createdAt: DateTime?) Task~Tuple~
    }

    class TransactionEntity {
        <<entity>>
        +Guid Id
        +string Name
        +string? Description
        +int UserId
        +int TransactionStatusId
        +int TransactionTypeId
        +int TransactionSituationId
        +int? CategorieId
        +DateTime PayDate
        +long Amount
        +int? CardId
        +int? BankId
        +int? SchedulerId
        +DateTime CreatedAt
        +DateTime? UpdatedAt
    }

    class TransactionView {
        <<view>>
        +Guid Id
        +string Name
        +string? Description
        +string Status
        +string Type
        +string Situation
        +string? Categorie
        +DateTime PayDate
        +long Amount
        +string? Card
        +string? Bank
        +string? Scheduler
        +DateTime CreatedAt
        +int Records
    }

    class TransactionSituationModel {
        <<repository>>
        -CoreReplica _coreReplica
        +GetTransactionSituationById(id: int, active: bool) Task~TransactionSituationEntity?~
        +GetTransactionSituationAsync(page: int, pageSize: int, active: bool) Task~Tuple~
    }

    class TransactionSituationEntity {
        <<entity>>
        +int Id
        +string Name
        +string? Description
        +bool Active
        +DateTime CreatedAt
        +DateTime? UpdatedAt
    }

    class TransactionSituationView {
        <<view>>
        +int Id
        +string Name
        +string? Description
        +bool Active
        +int Records
    }

    class PostPaymentTransactionRequest {
        +string Name
        +string Description
        +int TransactionStatus
        +int TransactionType
        +int TransactionSituation
        +int Categorie
        +DateTime PayDate
        +long Amount
        +int? Card
        +int? Bank
        +int? Scheduler
    }

    class PostPaymentTransactionValidatorAsync {
        +ValidateAsync(request: PostPaymentTransactionRequest) Task~ValidationResult~
    }

    class GetPaymentTransactionSituationRequest {
        +bool Active
        +int Page
        +int PageSize
    }

    class GetPaymentTransactionSituationAsyncResponse {
        +int Id
        +string Name
        +string? Description
        +bool Active
    }

    class GetPaymentTransactionSituationValidatorAsync {
        +ValidateAsync(request: GetPaymentTransactionSituationRequest) Task~ValidationResult~
    }

    class EnumTransactionSituation {
        <<enumeration>>
        +ENTRADA = 1
        +SAIDA = 2
    }

    PaymentController --> IPaymentService : delegates
    PaymentService ..|> IPaymentService : implements
    PaymentService --> TransactionModel : uses
    PaymentService --> TransactionSituationModel : uses
    PaymentService ..> EnumTransactionSituation : evaluates
    TransactionEntity <|-- TransactionModel : inherits
    TransactionSituationEntity <|-- TransactionSituationModel : inherits
    TransactionModel ..> TransactionView : queries
    TransactionSituationModel ..> TransactionSituationView : queries
    PaymentService ..> PostPaymentTransactionRequest : receives
    PaymentService ..> PostPaymentTransactionValidatorAsync : validates with
    PaymentService ..> GetPaymentTransactionSituationRequest : receives
    PaymentService ..> GetPaymentTransactionSituationValidatorAsync : validates with
    PaymentService ..> GetPaymentTransactionSituationAsyncResponse : returns
```
