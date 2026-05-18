# Portugues - BR

# Introdução

O produto foi idealizado mediante um conflito pessoal de seu programador, onde existia a necessitava registrar seu fluxo de gastos diários para entender seu custo de vida e caixa.

O software foi desenvolvido para substituir uma planilha utilizada anteriormente pelo programador localizada no Google drive, que continha abas de gráficos dinâmicos e celulas para inserir valores. A Problematica da planilha são as funções que por muitas vezes podiam gerar erros por estarem fortemente ligadas aos gráficos e a necessidade constante de trocar de planilha a cada ano.

Podemos concluir que a planilha carecia de consistencia de dados e flexibilidade na manutenção.

## O porque das tecnologias?

Antes de verificamos as escolhas de Stack (estrutura e tecnologias de um produto), nos é exigido o entendimento do software para fins acadêmicos e sem investimento prévio, limitando as escolhas de tecnologias do seu desenvolvedor.

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
        git --> gitA
  end
 subgraph K8S["K8S"]
        pods
        pods --> py["Python / flask"]
        pods --> kong["Kong API Gateway"]
        kong --> rateLimit["Rate limit acess"]
        kong --> auth["Auth"]
  end
 subgraph Backend["Backend"]
        vmB["Servidor dedicado"] --> Dotnet["Dotnet"]
        cluster["Cluster dedicado"] --> K8S
  end

 subgraph Database["Database"]
    subgraph Cluster["Cluster"]
        dbCore[("Postgres-A")]
        dbCoreRR[("Postgres-B")]
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

As tecnologias escolhidas de programação

* C#
* Python
    flask
* JavaScript
    Node
    React

### Infraestrutura

As aplicação utiliza cluster de banco de dados e kubernetes, ambos executados em containers Docker em rede interna

As imagens do container serão as da comunidade, como no caso do Redish ou Clickhouse, algumas exceções poderam utilizar imagens padrões como ubuntu, em ambos os casos os S.O constará

* Ubuntu Server
* Debian Bookworm
* CentOS

### Front-end

Desenvolvido em React com Next, provisionado em maquina virtual Debian Bookworm.

### Back-end

Estrutura composta por

* Monólito em C# (core)
* Dois microserviços
    Python, como serviço de dados
    C#, como serviço de login

Os serviços não devem ter comunicação aberta totalmente, é possível comunicação interna entre os serviços com endpoints privados, esse isolamento é parte da arquitetura de microserviços, o unico meio externo de comunicação entre eles deverá ser as chamadas do front-end para KongAPI com uso dos endpoints publicos.

Por possuir dois microserviços e monolito, a arquitetura é híbrida

Para evitar ataques o serviço conta com um rateLimit do kong.

### Banco de dados

* ClickHouse, banco analítico
* Redis, banco em cache
* Postgres, banco relacional com replica

### Consideraçoes

* Com essa arquitetura é possivel ter problemas pontuais em serviços distintos, priorizando o serviço Core.
* O serviço Core é um POO.
* O KongAPI poderá ser integrado com Grafana/Prometheus
* O serviço é local, por isso não é provisionado uma rede adequada.
* O Python foi escolhido para lidar com operações matemáticas e analíticas, devido ao seu desempenho excepcional e sólido. Sua principal desvantagem é o alto custo computacional e a dificuldade em lidar com muitas requisições simultâneas.
* O C# para ser o Core da aplicação por sua consistencia e resiliencia, sua principal desvantagem é a curva de aprendizado e verbosidade.
* O JavaScript para Front-end é padrão de mercado e baixa curva de aprendizado, suas desvantagem é o tempo médio de resposta inferior a tecnologias concorrentes como PHP, No front-end pode sobrecarregar client-side se for mal otimizado.
* O Postgres por sua estrutura e comportamentos com relação a segmentação das bases e possuir uma arquitetura que reforça o uso de schemas, é ótimo para altas requisições porem não lida muito bem com grandes volumes de dados e procedures internas.
* Redis é padrão de mercado é possui alta compatibilidade com Kong, possui tempo de resposta extremamente agíl, sua desvantagem é a falta de atualização pois nas versões mais novas é pago
* ClickHouse é um banco colunar com um dos maiores desempenhos atuais, é pessímo para transações de alterações de dados porem lida extremamente bem com alto carga de dados em suas tabelas.

### Tabela matrix de comparação programação

| Categoria     | Python| C#    | JavaScript |
|---------------|-------|-------|-------|
| Facilidade    | 10    | 6     | 8     |
| Velocidade    | 10    | 4     | 6     |
| Desempenho    | 7     | 9     | 6     |
| Comunidade    | 10    | 7     | 10    |
| Documentação  | 10    | 10    | 8     |

### Tabela matrix de comparação SGBD

| Categoria     | Redis | Postgres  | ClickHouse    |
|---------------|-------|-------|-------|
| Facilidade    | 6     | 8     | 6     |
| Velocidade    | 10    | 6     | 9     |
| Desempenho    | 10    | 6     | 9     |
| Escalabilidade| 5     | 8     | 8     |
| Comunidade    | 6     | 10    | 4     |
| Custo         | 0     | 0     | 0     |

# English - EUA

# Introduction

The product was conceived through a personal conflict of its developer, who needed to record his daily expenditure flow to understand his cost of living and cash flow.

The software was developed to replace a spreadsheet previously used by the programmer located on Google Drive, which contained dynamic chart tabs and cells for entering values. The problem with the spreadsheet was the functions, which could often generate errors because they were strongly linked to the charts, and the constant need to change spreadsheets every year.

We can conclude that the spreadsheet lacked data consistency and maintenance flexibility.

## Why the technologies?

Before checking the Stack choices (structure and technologies of a product), we are required to understand the software for academic purposes and without prior investment, limiting the developer's technology choices.

```mermaid
flowchart TD

 local["Local Machine"]

 subgraph Frontend["Frontend"]
        next["React / Next.js"]
        tailWind["Css / Tailwind"]
        vmF["Dedicated Server"] --> next
        vmF --> tailWind
  end
 subgraph Cicd["CD-CI"]
        git["Github"]
        gitA["Github Actions"]
        git --> gitA
  end
 subgraph K8S["K8S"]
        pods
        pods --> py["Python / Flask"]
        pods --> kong["Kong API Gateway"]
        kong --> rateLimit["Rate limit access"]
        kong --> auth["Auth"]
  end
 subgraph Backend["Backend"]
        vmB["Dedicated Server"] --> Dotnet["Dotnet"]
        cluster["Dedicated Cluster"] --> K8S
  end

 subgraph Database["Database"]
    subgraph Cluster["Cluster"]
        dbCore[("Postgres-A")]
        dbCoreRR[("Postgres-B")]
    end

  dbData[(ClickHouse)]
  dbMemory[(Redis)]

  docker["Docker Compose"] --> Cluster
  docker --> dbData
  docker --> dbMemory

  vmC["Dedicated Server"] --> docker
  
 end

local --> Backend
local --> Frontend
local --> Database

Backend --> Cicd
Frontend --> Cicd
Database --> Cicd
```

Chosen programming technologies:

* C#
* Python
    Flask
* JavaScript
    Node
    React

### Infrastructure

The application uses database clusters and Kubernetes, both running in Docker containers on an internal network.

The container images will be from the community, as in the case of Redis or Clickhouse; some exceptions may use standard images such as Ubuntu. In both cases, the OS will consist of:

* Ubuntu Server
* Debian Bookworm
* CentOS

### Front-end

Developed in React with Next, provisioned on a Debian Bookworm virtual machine.

### Back-end

Structure composed of:

* C# Monolith (core)
* Two microservices:
    Python, as a data service
    C#, as a login service

Services should not have fully open communication; internal communication between services is possible with private endpoints. This isolation is part of the microservices architecture. The only external means of communication between them should be frontend calls to KongAPI using public endpoints.

Since it has two microservices and a monolith, the architecture is hybrid.

To prevent attacks, the service relies on Kong's rateLimit.

### Database

* ClickHouse, analytical database
* Redis, cache database
* Postgres, relational database with replica

### Considerations

* With this architecture, it is possible to have specific problems in different services, prioritizing the Core service.
* The Core service is OOP (Object-Oriented Programming).
* KongAPI can be integrated with Grafana/Prometheus.
* The service is local, so an adequate network is not provisioned.
* Python was chosen to handle mathematical and analytical operations due to its exceptional and solid performance. Its main disadvantage is high computational cost and difficulty in handling many simultaneous requests.
* C# was chosen as the Core of the application for its consistency and resilience. Its main disadvantage is the learning curve and verbosity.
* JavaScript for the Front-end is a market standard with a low learning curve. Its disadvantage is the average response time being lower than competing technologies like PHP. On the front-end, it can overload the client-side if poorly optimized.
* Postgres, due to its structure and behavior regarding database segmentation and having an architecture that reinforces the use of schemas, is great for high requests but does not handle large volumes of data and internal procedures very well.
* Redis is a market standard and has high compatibility with Kong, with extremely agile response time. Its disadvantage is the lack of updates since the newer versions are paid.
* ClickHouse is a columnar database with one of the best current performances. It is poor for data alteration transactions but handles high data loads in its tables extremely well.

### Programming Comparison Matrix

| Category      | Python| C#    | JavaScript |
|---------------|-------|-------|-------|
| Ease          | 10    | 6     | 8     |
| Speed         | 10    | 4     | 6     |
| Performance   | 7     | 9     | 6     |
| Community     | 10    | 7     | 10    |
| Documentation | 10    | 10    | 8     |

### Database Comparison Matrix

| Category      | Redis | Postgres  | ClickHouse    |
|---------------|-------|-------|-------|
| Ease          | 6     | 8     | 6     |
| Speed         | 10    | 6     | 9     |
| Performance   | 10    | 6     | 9     |
| Scalability   | 5     | 8     | 8     |
| Community     | 6     | 10    | 4     |
| Cost          | 0     | 0     | 0     |
