# Procedimentos Operacionais e Ambiente de Desenvolvimento

Esta seção fornece instruções detalhadas para preparar, configurar, executar e manter o ambiente local de desenvolvimento do **Nievo EasyFin**.

---

## 🛠️ 1. Requisitos Prévios do Sistema

Antes de iniciar, certifique-se de ter as seguintes ferramentas instaladas em seu ambiente (Linux / macOS / Windows Subsystem for Linux):

- **Docker Engine** (v24.0+) e **Docker Compose** (v2.20+)
- **.NET 10 SDK** (ou .NET 8 SDK mínimo)
- **Node.js** (v18.x ou v20.x LTS) e **NPM** (v9.x+)
- **Python 3.10+** e `venv` (para os serviços de dados e migrações Alembic)
- **Make** (utilitário de automação de tarefas)

---

## ⚙️ 2. Configuração de Variáveis de Ambiente

O projeto disponibiliza modelos de variáveis de ambiente (`.env-example`). Para criar os arquivos `.env` em todos os módulos automaticamente:

```bash
make envs
```

Este comando executará a cópia dos modelos para os diretórios:
- `backend/.env`
- `frontend/web/.env`
- `infraestrutura/docker/.env`

---

## 🚀 3. Inicialização dos Serviços em Desenvolvimento

A execução da aplicação é simplificada por comandos declarativos no `makefile`:

### Etapa 1: Subir a Infraestrutura (Bancos + Gateway)
Inicia o PostgreSQL (master e replica), ClickHouse (2 nós + Zookeeper), Redis e o Kong API Gateway:

```bash
make infra-up
```

### Etapa 2: Executar os Microsserviços Backend (.NET)
Em terminais separados (ou em segundo plano), execute os serviços em modo `dotnet watch` para recompilação automática durante edições de código:

- **Microsserviço de Autenticação (`NievoEasyFin.Auth`):**
  ```bash
  make dotnet-run-auth
  ```
- **Monólito Core (`NievoEasyFin.Core`):**
  ```bash
  make dotnet-run-core
  ```

### Etapa 3: Executar o Frontend Web (React / Vite)
Instala as dependências e inicia o servidor de desenvolvimento do Vite com HMR na porta `5173`:

```bash
make web-exec
```

---

## 🧪 4. Execução de Suíte de Testes e CI/CD

### Execução Local:
Para rodar a suíte completa de testes unitários e de integração do backend:

```bash
make dotnet-test
# ou diretamente via CLI .NET:
dotnet test backend/NievoEasyFin.Tests/test.csproj
```

### Integração Contínua (CI/CD no GitHub Actions):
- O repositório possui uma pipeline automatizada em `.github/workflows/backend-tests.yml` que é acionada em todo **Pull Request**.
- A pipeline provisiona o runner `ubuntu-latest`, instala o .NET 10.0.x, restaura pacotes NuGet, compila a suíte e executa todos os testes de `test.csproj` com nível de verbosidade normal.
- **Configuração de Ambiente dos Testes:** Os testes utilizam a classe auxiliar `NievoEasyFin.Tests/Mocks/Helpers/TestEnvironment.cs`, que injeta em memória todas as variáveis de ambiente necessárias (JWT Secret, conexões de banco de teste, etc.), eliminando a necessidade de arquivos físicos `.env` no ambiente de CI.

---

## 📚 5. Servidor Local da Documentação (MkDocs)

Para visualizar e testar esta documentação localmente via container Docker:

```bash
make docs-up
```

Acesse a documentação no seu navegador no endereço: `http://localhost:6030`. Para encerrar o container da documentação:

```bash
make docs-down
```