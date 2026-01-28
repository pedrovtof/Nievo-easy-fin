# Easy Fin

## Objetivo

Serviço de controle de orçamento pessoal.

## Problemática

O aumento expressivo das casas de aposta (30 bilhões por mês de acordo com Banco Central) e perda contínua do Real frente ao Dólar (2,43 em 2005 para 4,95 em 2024, mais de 100% de desvalorização em 2025), confirmam o fato que a população Brasileira precisa de meios para gestão de orçamento.

## Público alvo

Famílias de baixa ou média renda com interesse em gestão financeira.

## Solução

Plataforma web PaaS, a plataforma deverá permitir registros de metas, demonstrativo da evolução ao longo do período, armazenar gastos do mês com relação às categorias como Mercado, Jogos, Cuidados pessoais entre outros com intenção de identificar padrões de gastos.

1. Funcionalidades principais
    * Login com e-mail (com suporte a SSO)
    * Login local
    * Reset senha
    * Registro e histórico de despesas/receitas
    * Criação de SubCategorias/tags personalizadas
    * Previsão de gastos por categoria
    * Gráficos interativos e insights mensais
    * Cadastramento de contas diversas
    * Definição de metas por categoria
    * Filtro por tags/contas/periodo/categoria/metas
2. Funcionalidade opcionais
    * Envio de alertas e relatórios por e-mail (via SMTP)
    * Integração com Open Finance (bancos e instituições financeiras)
    * Conexão com APIs de mercado financeiro e criptos
    * Monitoramento da inflação
    * Conversão e valorização cambial (Real x Dólar)

### Diagramas

#### Caso de uso

##### Serviço Login

![alt text](./images/caso_uso_login.png)

##### Serviço Core

![alt text](./images/caso_uso_core.png)

##### Serviço Data

![alt text](./images/caso_uso_data.png)

### Diagrama classes

### Diagramas de sequência

#### Login (atualizar)

```mermaid
sequenceDiagram
    participant Usuário
    participant Frontend
    participant GoogleSSO
    participant apiGateway
    participant AuthService
    participant DB

    Usuário->>Frontend: Clica em "Login"
    Frontend->>GoogleSSO: Redireciona para login Google
    GoogleSSO-->>Frontend: Retorna token/autorização
    Frontend->>apiGateway: Envia token
    apiGateway->>AuthService: Requisição chega no serviço
    AuthService->>GoogleSSO: Valida token
    GoogleSSO-->>apiGateway: Dados do usuário
    apiGateway->>AuthService: 
    AuthService->>DB: Verifica se usuário existe
    DB-->>AuthService: Usuário encontrado?
    alt Usuário não existe
        AuthService->>DB: Registra novo usuário
    end
    AuthService->>DB: Registra token/sessão
    DB-->>AuthService: Confirmação
    AuthService-->>Frontend: Retorna sessão/token
    Frontend-->>Usuário: Acesso liberado
```

#### Transacao/grupo/categoria/tag/conta (atualizar)

```mermaid
    sequenceDiagram
    participant Usuário
    participant Frontend
    participant apiGateway
    participant CoreService
    participant DB

    Usuário->>Frontend: Preenche formulário
    Frontend->>apiGateway: Envia dados para validação
    apiGateway->>CoreService: envia dados para serviço

    alt Formulário inválido
        CoreService-->>Frontend: Retorna erro de validação
        Frontend-->>Usuário: Exibe erro
    else Formulário válido
        CoreService->>DB: Verifica duplicidade
        alt Registro não existe
            CoreService->>DB: Persiste dados
            DB-->>CoreService: Confirmação de criação
            CoreService-->>Frontend: Sucesso
            Frontend-->>Usuário: Exibe sucesso
        else Registro já existe
            CoreService-->>Frontend: Retorna aviso de item existente
            Frontend-->>Usuário: Exibe mensagem de duplicidade
        end
    end

```

#### visualizacao (atualizar)

```mermaid
 sequenceDiagram
    participant Usuário
    participant Frontend
    participant apiGateway
    participant DataService
    participant DB

    Usuário->>Frontend: Solicita previsão
    Frontend->>apiGateway:  Solicita dados históricos
    apiGateway->>DataService: Requisição chega no serviço
    DataService->>DB: Consulta dados financeiros
    DB-->>DataService: Retorna dados brutos

    DataService->>DataService: Limpeza e tratamento dos dados
    DataService->>DataService: Executa algoritmos de previsão (ex: média, regressão)

    DataService-->>Frontend: Retorna dados previstos
    Frontend-->>Usuário: Exibe gráfico e informações
```


### Diagrama componentes

```mermaid
flowchart TD
 subgraph Frontend["Frontend"]
        next["React"]
  end

 subgraph Backend["Backend"]
    subgraph K8S["K8S"]
      node["Node.js API"]
      python["Python API"]
      kong["API GATEWAY"]
    end

    subgraph VM["VM"]
      dotnet["Dotnet C#"]
    end
  end

  subgraph Database["Database"]
    subgraph Cluster["Cluster"]
      dbCore[("Postgres")]
      dbCoreRR[("Postgres-rr")]
    end
    dbData[(ClickHouse)]
    dbMemory[(Redish)]
  end

    Frontend -- HTTP --> kong
    kong-- HTTP --> dbMemory
    kong-- HTTP --> python
    kong-- HTTP --> node
    kong-- HTTP --> dotnet
    Backend -- JDBC --> Database
    python -- HTTP --> dbData
```

### Diagrama de implantação

```mermaid
flowchart TD

 local["Maquina local"]

 subgraph Frontend["Frontend"]
        next["React / Next.js"]
        tailWind["Css / Tailwind"]
        vmF["Servidor dedicado"] --> next
        vmF --> tailWind
  end
 subgraph Cicd["CD-CI"]
        git["Github"]
        gitA["Github Actions"]
        vercel["Vercel"]
        git --> gitA
        gitA --> vercel
  end
 subgraph K8S["K8S"]
        pods
        pods --> py["Python / flask"]
        pods --> node["Node.js / express"]
        pods --> kong["Kong API Gateway"]
        kong --> rateLimit["Rate limit acess"]
        kong --> auth["Auth"]
  end
 subgraph Backend["Backend"]
        vmB["Servidor dedicado"] --> dotnet["Dotnet"]
        cluster["Cluster dedicado"] --> K8S
  end

 subgraph Database["Database"]
    subgraph Cluster["Cluster"]
        dbCore[("Postgres")]
        dbCoreRR[("Postgres-rr")]
    end

  dbData[(ClickHouse)]
  dbMemory[(Redis)]

  docker["Docker compose"] --> Cluster
  docker --> dbData
  docker --> dbMemory

  vmC["Servidor dedicado"] --> docker
  
 end

local --> Backend
local --> Frontend
local --> Database

Backend --> Cicd
Frontend --> Cicd
Database --> Cicd
```

### Diagrama SGBD

#### Postgres

##### Database Core

![Database Core](./images/db_diagram_core.png)

##### Database Signup

![Database Signup](./images/db_diagram_signup.png)

### Diagrama de fluxos

Modelos de fluxo ainda não criados

#### Modelo de desing

Modelo ainda não criado

### Concorrentes

* [Kinvo](https://kinvo.com.br/)
* [Minhas Economias](https://minhaseconomias.com.br/)
* [Meu Dinheiro - Sistema para controle financeiro pessoal e empresarial](https://www.meudinheiroweb.com.br/)
* [Organizze: Controle Financeiro Pessoal - Organizador de contas](https://www.organizze.com.br/#recursos)
* [Wisecash Web](https://www.wisecashapp.com.br/#/site)

### Fontes

[Gasto com apostas online chegam a 30 bilhoes| infomoney](https://www.infomoney.com.br/politica/gastos-com-apostas-online-chegam-a-r-30-bilhoes-por-mes-e-acendem-alerta-no-bc/)

[Crise reduz poder de compra e muda perfil de consumo da classe média | Economia | G1](https://g1.globo.com/economia/noticia/2021/04/24/crise-reduz-poder-de-compra-e-muda-perfil-de-consumo-da-classe-media.ghtml)