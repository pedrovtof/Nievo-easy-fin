# Nievo EasyFin - Documentação Oficial

Bem-vindo à documentação pública e técnica do **Nievo EasyFin**, plataforma de gestão financeira pessoal e empresarial desenvolvida com arquitetura híbrida de alto desempenho.

O objetivo principal desta documentação é apresentar detalhadamente toda a estrutura do sistema, seus microsserviços, modelos de persistência poliglota, fluxos operacionais, contratos de API e diretrizes de desenvolvimento.

---

## 🗺️ Mapa da Documentação

### 🏛️ 1. Arquitetura do Sistema
- **[Visão Geral](content/ArquiteturaSistema/intro.md):** Arquitetura híbrida (Monólito Core C# + Microsserviço de Auth C# + Microsserviço Data em Python Flask + Frontend React/Vite).
- **[Banco de Dados e Esquemas](content/ArquiteturaSistema/banco_de_dados.md):** Persistência poliglota com PostgreSQL (master-replica), Redis (cache e rate-limit) e ClickHouse (banco colunar analítico OLAP). Esquemas `journey` e `bank`.
- **[Gateway e Infraestrutura](content/ArquiteturaSistema/infraestrutura.md):** Roteamento centralizado via Kong API Gateway, rate limiting, autenticação JWT, suporte CORS e clusters em Docker Compose/Kubernetes.

### 🔄 2. Fluxos de Negócio
- **[Visão Geral](content/FluxosNegocio/intro.md):** Resumo executivo das jornadas do usuário.
- **[Sobre e Casos de Uso](content/FluxosNegocio/about.md):** Objetivos do produto, público-alvo, comparativo de mercado e diagramas UML/Mermaid atualizados (Casos de Uso, Sequência e Implantação).
- **[Onboarding e Aceite de Termos](content/FluxosNegocio/onboarding_termos.md):** Registro tradicional e via SSO Google, fluxo de e-mail de validação e auditoria de aceite de termos (`journey.users_accepted_terms`).
- **[Autenticação e Segurança](content/FluxosNegocio/autenticacao_seguranca.md):** Fluxo de login, tokens JWT com claims personalizadas, hashing PBKDF2 e fluxo de redefinição de senha por PIN numérico de 6 dígitos via SMTP.
- **[Gestão de Contas e Cartões](content/FluxosNegocio/gestao_contas_cartoes.md):** Vínculo de usuários com instituições financeiras (`UserBank`) e cartões de crédito/débito (`UserBankCard`).

### 💻 3. Frontend
- **[Arquitetura e Componentes](content/Frontend/intro.md):** Migração Next.js/Tailwind para React 18 + Vite + MUI (Material UI), padrão de arquivos (`index`, `View`, `styles`, `api`), rotas protegidas e públicas, gerenciamento de temas e guias de suporte a dados simulados (`mockData`).

### 🔌 4. APIs e Integrações
- **[Visão Geral e Kong Gateway](content/ApiIntegracoes/intro.md):** Padrões RESTful, headers obrigatórios (`Host`, `User-Agent`, `Authorization`), controle de versão de API e respostas padronizadas (`ResponseApiSucess`, `ResponseApiError`).
- **[Endpoints de Autenticação (Auth)](content/ApiIntegracoes/auth_endpoints.md):** Especificação completa das rotas públicas e administrativas do microsserviço de autenticação.
- **[Endpoints de Contas e Cartões (Core)](content/ApiIntegracoes/core_endpoints.md):** Especificação completa das rotas públicas e privadas do microsserviço Core.

### 🛠️ 5. Procedimentos Operacionais
- **[Ambiente e Desenvolvimento](content/ProcedimentosOperacionais/intro.md):** Requisitos, configuração de variáveis de ambiente (`.env`) e comandos do `Makefile`.
- **[Docker, Clusters e Migrações](content/ProcedimentosOperacionais/docker_migracoes.md):** Execução do Docker Compose (`infra-up`), configuração do cluster PostgreSQL master-replica, cluster ClickHouse com Zookeeper, migrações Alembic/EF Core e servidor local de documentação MkDocs.

### 📏 6. Boas Práticas e Padrões Técnicos
- **[Padrões Técnicos e Arquitetura](content/BoasPraticas/intro.md):** Organização do código, separação de camadas, FluentValidation, convenções de código C# e React, e padrão de testes unitários (AAA & Objeto_Estado_Resultado).

### 📜 7. Atualizações e Histórico
- **[Histórico de Alterações (Changelog)](content/AtualizacoesHistorico/intro.md):** Registro de todas as entregas, melhorias arquiteturais e notas de versão.

---

## ⚡ Atalhos Rápidos

```bash
# Subir infraestrutura completa (Bancos + Gateway)
make infra-up

# Executar os serviços backend em modo watch
make dotnet-run-auth
make dotnet-run-core

# Executar o frontend React/Vite
make web-exec

# Subir servidor local da documentação MkDocs (porta 6030)
make docs-up
```
