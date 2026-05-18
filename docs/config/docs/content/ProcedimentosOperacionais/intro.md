# Procedimentos Operacionais

Esta seção descreve as etapas necessárias para configurar, executar e manter o ambiente do Nievo Easy Fin.

## Ambiente de Desenvolvimento

O projeto utiliza um `makefile` para simplificar as operações comuns.

### 1. Requisitos Prévios

*   Docker e Docker Compose
*   .NET 8 SDK (ou superior)
*   Node.js e NPM (para o frontend)
*   Python 3.x (para microserviços de dados)

### 2. Configuração Inicial

Clone o repositório e configure as variáveis de ambiente:
1.  Copie o `backend/env-example.txt` para um arquivo `.env` na raiz do projeto e nas pastas dos serviços correspondentes.
2.  Ajuste as credenciais de banco de dados e chaves de API conforme necessário.

### 3. Execução da Infraestrutura

Para subir os bancos de dados (Postgres, ClickHouse, Redis) e o API Gateway (Kong):

```bash
make infra-up
```

### 4. Execução dos Serviços Backend

Os serviços backend podem ser executados via `dotnet watch` para facilitar o desenvolvimento:

*   **Auth Service:** `make dotnet-run-auth`
*   **Core Service:** `make dotnet-run-core`

### 5. Execução do Frontend

Para iniciar o servidor de desenvolvimento do frontend:

```bash
make web-exec
```

## Manutenção e Deploy

### Migrações de Banco de Dados

*   **Postgres:** As migrações são gerenciadas pelo Entity Framework Core (no Monólito) ou Alembic (nos microserviços).
*   **ClickHouse:** Esquemas de tabelas devem ser aplicados manualmente ou via scripts de inicialização no Docker.

### Comandos Úteis do Makefile

*   `make docs-up`: Sobe a documentação (MkDocs) via Docker.
*   `make dotnet-test`: Executa os testes unitários do backend.
*   `make docker-nievo`: Sobe toda a aplicação (Infra + App) via Docker Compose.