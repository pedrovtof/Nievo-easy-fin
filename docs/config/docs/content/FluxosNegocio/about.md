# Sobre o Produto e Casos de Uso

---

## 🎯 Objetivo

Plataforma PaaS para controle financeiro pessoal e de pequenos empreendedores, criada para substituir planilhas manuais propensas a erros de cálculo e de manutenção anual.

---

## 👥 Público-Alvo

Famílias e pequenos empreendedores com necessidade de centralizar a gestão de caixa, prever custos mensais, categorizar despesas e monitorar o atingimento de metas financeiras.

---

## 💡 Funcionalidades do Produto

### 1. Funcionalidades Principais (Core e Auth)
- **Autenticação:** Cadastro tradicional por e-mail/senha e login social via SSO Google.
- **Validação e Segurança:** Confirmação de e-mail por PIN temporário e redefinição de senha com envio por SMTP.
- **Auditoria de Termos:** Obrigatoriedade de aceite dos Termos de Uso com registro de IP/Host e User-Agent.
- **Contas Bancárias:** Vínculo de contas do usuário a instituições financeiras cadastradas (`UserBank`).
- **Cartões de Crédito/Débito:** Gestão de cartões por banco, tipo de cartão e bandeira (`UserBankCard`).
- **Gestão Financeira:** Registro de receitas e despesas com categorização e tags.
- **Metas e Orçamentos:** Definição de limites orçamentários por categoria com acompanhamento em tempo real.
- **Analytics e Dashboards:** Visualização de gráficos dinâmicos de consumo e previsões históricas.

### 2. Funcionalidades Opcionais e Futuras
- Notificações e alertas por e-mail de metas excedidas.
- Integração com Open Finance (PIX e conciliação bancária automática).
- Cotação de moedas e valorização cambial (BRL / USD / EUR).
- Monitoramento de inflação e rendimento de investimentos.

---

## 🖼️ Diagramas do Sistema

### 1. Casos de Uso

#### Serviço de Autenticação (Auth)
![Caso de Uso Login](../../images/caso_uso_login.png)

#### Serviço Core (Contas e Transações)
![Caso de Uso Core](../../images/caso_uso_core.png)

#### Serviço de Dados (Analytics)
![Caso de Uso Data](../../images/caso_uso_data.png)

---

### 2. Diagramas de Sequência

#### Fluxo de Autenticação e SSO

```mermaid
sequenceDiagram
    participant Usuario as Usuário
    participant Frontend as Frontend (React / Vite)
    participant GoogleSSO as Google OAuth2
    participant Kong as Kong API Gateway
    participant Auth as AuthService (C#)
    participant DB as Postgres (journey)

    Usuario->>Frontend: Clica em "Entrar com Google"
    Frontend->>GoogleSSO: Solicita autenticação OAuth
    GoogleSSO-->>Frontend: Retorna access_token
    Frontend->>Kong: POST /api/public/v1/Authenticator/singin-sso (token)
    Kong->>Auth: Encaminha requisição
    Auth->>GoogleSSO: Valida token e verifica client_id (aud)
    GoogleSSO-->>Auth: Retorna dados do usuário (email, sub, name)
    Auth->>DB: Consulta vínculo na tabela journey.user_provider_sso
    alt Usuário cadastrado
        DB-->>Auth: Retorna entidade do usuário
    else Usuário novo
        Auth->>DB: Registra usuário e vínculo SSO
        DB-->>Auth: Confirmação de persistência
    end
    Auth->>Auth: Emite JWT assinado (HS256)
    Auth-->>Frontend: Retorna Token JWT e status 200 OK
    Frontend-->>Usuario: Acesso liberado ao Dashboard
```

#### Fluxo de Cadastro de Conta Bancária e Cartão

```mermaid
sequenceDiagram
    participant Usuario as Usuário
    participant Frontend as Frontend (React / Vite)
    participant Kong as Kong API Gateway
    participant Core as CoreService (C#)
    participant DB as Postgres (bank)

    Usuario->>Frontend: Seleciona Banco e insere apelido da conta
    Frontend->>Kong: POST /api/public/v1/Accounts/user-banks (Bearer JWT)
    Kong->>Kong: Valida JWT token
    Kong->>Core: Encaminha DTO + claim email
    Core->>DB: Verifica existência do banco e usuário
    alt Banco e Usuário válidos
        Core->>DB: Insere registro em bank.user_banks
        DB-->>Core: Confirmação de criação
        Core-->>Frontend: Retorna HTTP 200 OK (Criado com Sucesso)
        Frontend-->>Usuario: Exibe confirmação na tela
    else Erro de validação ou item duplicado
        Core-->>Frontend: Retorna HTTP 400 Bad Request
        Frontend-->>Usuario: Exibe mensagem de erro
    end
```

#### Fluxo de Consulta Analítica de Gastos

```mermaid
sequenceDiagram
    participant Usuario as Usuário
    participant Frontend as Frontend (React / Vite)
    participant Kong as Kong API Gateway
    participant DataService as DataService (Python / Flask)
    participant ClickHouse as ClickHouse OLAP Cluster

    Usuario->>Frontend: Acessa aba de Insights e Previsões
    Frontend->>Kong: GET /api/data/v1/analytics/predict (Bearer JWT)
    Kong->>DataService: Roteia requisição
    DataService->>ClickHouse: Executa query de agregação temporal (ReplicatedMergeTree)
    ClickHouse-->>DataService: Retorna série temporal de despesas
    DataService->>DataService: Executa modelo de média / regressão de tendência
    DataService-->>Frontend: Retorna JSON com dados históricos e previstos
    Frontend-->>Usuario: Renderiza gráfico dinâmico no Dashboard
```

---

### 3. Diagrama de Componentes

```mermaid
flowchart TD
    subgraph ClientLayer["Camada de Apresentação"]
        ReactApp["React 18 / Vite / MUI (Single Page Application)"]
    end

    subgraph GatewayLayer["Camada de Entrada & Segurança"]
        KongGW["Kong API Gateway"]
        RedisLimit[("Redis (Rate Limiting & Session Cache)")]
        KongGW --> RedisLimit
    end

    subgraph ServiceLayer["Camada de Serviços (Backend)"]
        AuthApp["NievoEasyFin.Auth (C# .NET 10)"]
        CoreApp["NievoEasyFin.Core (C# .NET 10)"]
        DataApp["Data Service (Python Flask)"]
    end

    subgraph StorageLayer["Camada de Dados (Persistência Poliglota)"]
        PostgresDB[("PostgreSQL Master/Replica (journey & bank)")]
        ClickHouseDB[("ClickHouse OLAP Cluster")]
    end

    ReactApp -->|HTTP REST| KongGW
    KongGW -->|/api/auth| AuthApp
    KongGW -->|/api/core| CoreApp
    KongGW -->|/api/data| DataApp

    AuthApp -->|EF Core / Npgsql| PostgresDB
    AuthApp -->|StackExchange.Redis| RedisLimit

    CoreApp -->|EF Core / Npgsql| PostgresDB
    CoreApp -->|Cache Bancos| RedisLimit

    DataApp -->|HTTP Native / Driver| ClickHouseDB
```

---

### 4. Diagrama de Implantação (Deployment Diagram)

```mermaid
flowchart TD
    subgraph LocalMachine["Máquina Local / Ambiente Dev"]
        DockerCompose["Docker Compose Engine"]

        subgraph InfraContainers["Containers de Infraestrutura"]
            PostgresMasterC["postgres_nodea (Master Port 5432)"]
            PostgresReplicaC["postgres_nodeb (Replica Port 5433)"]
            ClickHouseAC["clickhouse_nodea (Port 8123)"]
            ClickHouseBC["clickhouse_nodeb (Port 8124)"]
            ZookeeperC["zookeeper (Port 2181)"]
            RedisC["redish (Port 6379)"]
            KongC["kong-cp (Port 8000)"]
        end

        subgraph AppContainers["Containers / Processos de Aplicação"]
            AuthC["NievoEasyFin.Auth (.NET 10 - Port 8081)"]
            CoreC["NievoEasyFin.Core (.NET 10 - Port 8082)"]
            DataC["Data Service (Python - Port 8083)"]
            WebC["Frontend React/Vite (Port 5173)"]
            DocsC["MkDocs Material (Port 6030)"]
        end
    end

    KongC --> AuthC
    KongC --> CoreC
    KongC --> DataC
    WebC --> KongC

    AuthC --> PostgresMasterC
    AuthC --> RedisC
    CoreC --> PostgresMasterC
    CoreC --> RedisC
    DataC --> ClickHouseAC

    ClickHouseAC --> ZookeeperC
    ClickHouseBC --> ZookeeperC
    PostgresMasterC -.-> PostgresReplicaC
```

---

### 5. Diagramas de Banco de Dados (SGBD)

#### PostgreSQL - Schema Core e Bank
![Database Core](../../images/db_diagram_core.png)

#### PostgreSQL - Schema Journey e Auth
![Database Signup](../../images/db_diagram_signup.png)

---

## 🥊 Concorrentes Analisados

- [Kinvo](https://kinvo.com.br/) - Foco em consolidação de investimentos.
- [Minhas Economias](https://minhaseconomias.com.br/) - Controle de orçamento doméstico simples.
- [Meu Dinheiro Web](https://www.meudinheiroweb.com.br/) - Gestão financeira pessoal e de microempresas.
- [Organizze](https://www.organizze.com.br/#recursos) - Interface minimalista para gestão de contas e cartões.
- [Wisecash Web](https://www.wisecashapp.com.br/#/site) - Aplicativo leve para controle de gastos diários.
