# Docker, Clusters e Migrações de Banco de Dados

Este documento descreve os procedimentos operacionais avançados para gestão da infraestrutura containerizada, replicação de bancos de dados e execução de migrações no **Nievo EasyFin**.

---

## 🐳 1. Gestão da Infraestrutura Containerizada

A infraestrutura completa é gerenciada via Docker Compose na pasta `infraestrutura/docker/`.

### 📌 Comandos Principais:

```bash
# Subir toda a infraestrutura em segundo plano
make infra-up

# Verificar o status dos containers ativos
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

# Visualizar logs em tempo real do API Gateway Kong
docker logs -f kong-cp

# Visualizar logs do PostgreSQL Master
docker logs -f postgres_nodea

# Derrubar a infraestrutura mantendo os volumes de dados
make infra-down
```

---

## 🗄️ 2. Cluster PostgreSQL e Replicação Streaming

O PostgreSQL utiliza uma arquitetura com dois nós:

1. **`postgres_nodea` (Master - Porta 5432):** Recebe todas as operações de escrita (INSERT, UPDATE, DELETE) e consultas de leitura.
2. **`postgres_nodeb` (Read-Replica - Porta 5433):** Recebe replicação contínua em tempo real via *Streaming Replication*.

### 🔄 Verificação do Status de Replicação:

Para confirmar se a réplica está sincronizada com o Master, execute a consulta no container Master:

```bash
docker exec -it postgres_nodea psql -U postgres -d postgres_nievo_easy_fin -c "SELECT client_addr, state, sync_state, sync_priority FROM pg_stat_replication;"
```

---

## 📊 3. Cluster ClickHouse e Zookeeper

O ClickHouse é utilizado para consultas analíticas pesadas (OLAP).

- **Nós Activos:** `clickhouse_nodea` (Porta HTTP 8123) e `clickhouse_nodeb` (Porta HTTP 8124).
- **Coordenação Zookeeper:** O serviço `zookeeper` escuta na porta 2181 e garante a sincronização das tabelas de motor `ReplicatedMergeTree`.

### 🔍 Teste de Conectividade com ClickHouse:

```bash
curl "http://localhost:8123/?query=SELECT%201"
```

---

## 📜 4. Migrações de Banco de Dados

As migrações de esquema são separadas por microsserviço:

### 🐍 Migrações via Alembic (Python) - Schemas `journey` e `bank`

Nos projetos `NievoEasyFin.Auth` e `NievoEasyFin.Core`, as alterações de esquema do PostgreSQL são versionadas via scripts Python do **Alembic**:

```bash
# Ativar o ambiente virtual de dependências Python
source backend/venv/bin/activate

# Aplicar migrações pendentes no microsserviço de Auth
cd backend/NievoEasyFin.Auth
alembic upgrade head

# Gerar uma nova migração após alterar entidades no Auth
alembic revision --autogenerate -m "nome_da_migracao"

# Aplicar migrações no microsserviço Core
cd ../NievoEasyFin.Core
alembic upgrade head
```

---

## 📖 5. Implantação e Build da Documentação (MkDocs Material)

A documentação é mantida com **MkDocs Material** e empacotada via container Docker.

```bash
# Iniciar o container da documentação localmente
make docs-up

# Compilar arquivos estáticos HTML na pasta site/
docker run --rm -v $(pwd)/docs/config:/docs squidfunk/mkdocs-material:9 build
```
