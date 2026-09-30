# Fluxo de Planejamento Financeiro: Metas e Categorias

O planejamento financeiro e a classificação de despesas no **Nievo EasyFin** são gerenciados pelo Monólito `NievoEasyFin.Core` operando sobre o esquema **`goals`** do PostgreSQL.

---

## 🎯 1. Gestão de Metas Financeiras (`goals.goals`)

As metas financeiras permitem que o usuário defina objetivos de economia (ex: Reserva de Emergência, Viagem, Compra de Imóvel) ou tetos orçamentários com prazos definidos.

```mermaid
flowchart TD
    User["Usuário no Frontend"] --> InputGoal["Preenche Nome, Descrição, Valor, % e Data de Expiração"]
    InputGoal --> PostGoal["POST /api/public/v1/Accounts/user:goal (Bearer JWT)"]
    
    PostGoal --> ValToken{"Token JWT Válido?"}
    ValToken -->|Não| HTTP401["HTTP 401 Unauthorized"]
    ValToken -->|Sim| ExtractEmail["Extrai claim de e-mail e busca user_details.user"]
    
    ExtractEmail --> UserFound{"Usuário existe e está ativo?"}
    UserFound -->|Não| HTTP404["HTTP 404 Not Found (Usuário não encontrado)"]
    
    UserFound -->|Sim| ValRules{"Validações de Domínio<br/>- Nome: 2-99 chars<br/>- Amount > 0<br/>- ExpireAt > hoje"}
    ValRules -->|Falha| HTTP400Val["HTTP 400 Bad Request (Parâmetros inválidos)"]
    
    ValRules -->|Sucesso| CheckGoalName{"Meta ativa com mesmo nome já existe?"}
    CheckGoalName -->|Sim| HTTP400Exists["HTTP 400 Bad Request (Meta já cadastrada)"]
    
    CheckGoalName -->|Não| SaveGoal["Persiste em goals.goals (GoalModel.CreateGoal)"]
    SaveGoal --> HTTP200["HTTP 200 OK (Meta criada com sucesso)"]
```

### 📋 Consulta de Metas
- Endpoint: `GET /api/public/v1/Accounts/user:goal`
- Suporta filtros por metas ativas/inativas (`active=true|false`) e paginação (`page`, `page_size`).
- Utiliza consulta Dapper otimizada com a *window function* `count(*) over() as Records` para obter o total de registros e os itens paginados em uma única viagem ao banco de réplica (`CoreReplica`).

---

## 🏷️ 2. Hierarquia e Associação de Categorias (`goals.category`)

As categorias financeiras organizam os lançamentos do usuário e podem ser associadas a uma meta ou estruturadas hierarquicamente (subcategorias).

### ⚖️ Regra de Negócio Crucial: Exclusividade Mútua
Uma categoria **só pode ser criada com exatamente um vínculo**:
1. **Vinculada a uma Meta (`goal > 0` e `parent_category == null`):** A categoria contribui diretamente para o orçamento ou teto da meta especificada.
2. **Vinculada a uma Categoria Pai (`parent_category > 0` e `goal == null`):** A categoria atua como uma subcategoria (ex: `Alimentação` -> `Supermercado`).
3. **Proibido:** Criar categoria sem meta e sem pai, ou fornecendo ambos simultaneamente (`HTTP 400 - POSTUSERCATEGORYASYNC_CORESERVICE_400_CATEGORY_ONLY_CAN_BE_CREATED_WITH_ONE_GOAL_OR_PARENT`).

```mermaid
flowchart TD
    User["Usuário no Frontend"] --> InputCat["Informa Nome, Descrição e Vínculo (Meta OU Categoria Pai)"]
    InputCat --> PostCat["POST /api/public/v1/Accounts/user:category (Bearer JWT)"]
    
    PostCat --> ValAuth{"Token JWT Válido?"}
    ValAuth -->|Não| HTTP401["HTTP 401 Unauthorized"]
    ValAuth -->|Sim| FindUserCat["Localiza usuário por e-mail"]
    
    FindUserCat --> CheckExclusive{"Vínculo Exclusivo Válido?<br/>(Goal XOR ParentCategory)"}
    CheckExclusive -->|Não| HTTP400Excl["HTTP 400 Bad Request (Apenas um vínculo permitido)"]
    
    CheckExclusive -->|Sim| HasParent{"Informou ParentCategory?"}
    HasParent -->|Sim| ValParent{"ParentCategory existe e está ativa?"}
    ValParent -->|Não| HTTP404Parent["HTTP 404 Not Found (Categoria pai não encontrada)"]
    ValParent -->|Sim| CheckCatName
    HasParent -->|Não| CheckCatName
    
    CheckCatName{"Nome de categoria já ativo para o usuário?"}
    CheckCatName -->|Sim| HTTP400CatExists["HTTP 400 Bad Request (Categoria já existe)"]
    
    CheckCatName -->|Não| SaveCategory["Persiste em goals.category (CategoryModel.CreateCategory)"]
    SaveCategory --> HTTP200Cat["HTTP 200 OK (Categoria criada com sucesso)"]
```

---

## 🌳 3. Estrutura em Árvore e Consulta Paginada

Ao listar as categorias do usuário via `GET /api/public/v1/Accounts/user:category`, o serviço realiza um `LEFT JOIN` entre `goals.category` e `goals.goals`:

```mermaid
graph TD
    subgraph Metas["Metas Financeiras (goals.goals)"]
        G1["Meta: Viagem Fim de Ano"]
        G2["Meta: Manutenção Veículo"]
    end

    subgraph Categorias["Categorias e Subcategorias (goals.category)"]
        C1["Categoria: Passagens Aéreas"] -->|goal_id| G1
        C2["Categoria: Hospedagem"] -->|goal_id| G1
        C3["Categoria: Transporte"] -->|goal_id| G2
        C4["Subcategoria: Combustível"] -->|parent_category| C3
        C5["Subcategoria: Pedágio"] -->|parent_category| C3
    end
```

### 🔍 Dados Retornados na Consulta (`UserCategoryView`):
- `id`: Identificador da categoria.
- `name` e `description`: Dados descritivos da categoria.
- `active`: Indicador se a categoria está em uso.
- `parent_category`: Identificador da categoria pai (caso seja subcategoria).
- `goal_name`: Nome da meta financeira associada (obtida via JOIN).
- `goal_description`: Descrição da meta financeira associada.
- `created_at` e `updated_at`: Carimbos de auditoria temporal.

---

## 📌 4. Tabelas do Esquema `goals` Utilizadas

- **`goals.goals`:**
  - `id`: Chave primária SERIAL.
  - `name`: Nome da meta (VARCHAR 150).
  - `description`: Detalhes do objetivo (VARCHAR 255).
  - `amount`: Valor monetário em centavos ou valor percentual.
  - `is_percent`: Flag booleano indicando se o valor representa percentual.
  - `expire_at`: Prazo final de atingimento da meta (DATE).
  - `user_id`: Identificador do usuário proprietário.
  - `active`: Estado ativo/inativo.
- **`goals.category`:**
  - `id`: Chave primária SERIAL.
  - `name`: Nome da categoria (VARCHAR 150).
  - `description`: Descrição (VARCHAR 255).
  - `user_id`: Usuário proprietário.
  - `goal_id`: Chave estrangeira referenciando `goals.goals(id)`.
  - `parent_category`: Chave auto-relacionada apontando para outra categoria.
  - `active`: Estado ativo/inativo.
