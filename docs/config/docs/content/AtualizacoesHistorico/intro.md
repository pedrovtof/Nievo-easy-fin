# Atualizações e Histórico

Este documento registra as principais milestones e alterações arquiteturais do projeto Nievo Easy Fin.

## Versão Atual: Alpha (Em desenvolvimento)

O projeto encontra-se em fase ativa de desenvolvimento, com as funcionalidades base de autenticação e core sendo consolidadas.

### 2026-06-05: Aceite de Termos de Uso no Cadastro de Usuários
*   **Novo fluxo:** O aceite dos Termos de Uso passou a ser **obrigatório** no cadastro tradicional (`POST /singup`) e no cadastro via SSO (`POST /singup-sso`).
*   **Novas entidades:** `AcceptTermsEntity` e `UsersAcceptedTermsEntity` para armazenar os termos ativos e o histórico de aceites por usuário.
*   **Novas tabelas no banco:** `journey.accept_terms` (termos cadastrados e versionados) e `journey.users_accepted_terms` (registro de aceite por usuário, com data, host e user-agent).
*   **Novos campos no request:** `accept_terms (bool)` obrigatório no body. `Host` e `User-Agent` obrigatórios nos headers (registrados no aceite para auditoria).
*   **Novas migrations Alembic:** criação das tabelas, seed inicial dos termos de cadastro e constraints de integridade referencial.
*   **Testes:** revalidação e extensão da suíte de testes — 106 testes passando, incluindo 6 novos testes de serviço e 8 novos cenários de BadRequest no controller.

### 2026-05-23: Migração do Framework e Biblioteca de UI do Frontend
*   **Framework:** Substituição do bundler **Next.js** pelo **Vite**, reduzindo o tempo de build e simplificando a configuração de desenvolvimento.
*   **Estilização/UI:** Remoção completa do **Tailwind CSS** e adoção do **MUI (Material UI)** como biblioteca de componentes, trazendo consistência visual e acessibilidade nativa.
*   **Refatoração:** Todos os componentes e páginas foram refatorados para o padrão de arquitetura modular (`index.jsx`, `View.jsx`, `styles.js`, `api.js`).
*   **Limpeza:** Remoção de arquivos órfãos legados (`ForgotPassword.jsx` flat, `App.css`, `assets/react.svg`) e dos arquivos de configuração do Tailwind (`postcss.config.js`, `tailwind.config.js`).

### 2026-05-18: Melhoria Abrangente de Documentação
*   **README:** Tradução completa para Inglês (EUA) e atualização dos diagramas de arquitetura.
*   **MkDocs:** Expansão detalhada das seções de Arquitetura, Fluxos de Negócio, Boas Práticas e Procedimentos Operacionais.
*   **Código:** Implementação sistemática de XML Docstrings em serviços e interfaces do backend para melhor inteligibilidade.
*   **Gestão:** Criação da estrutura `gemini/` para rastreamento de decisões de IA e logs de implementação.

### 2026-05-16: Refatoração Global de Naming
*   Execução de um rename global do namespace de `Nievo-easyfin` para `NievoEasyFin` para garantir consistência com os padrões de nomenclatura C# e PascalCase.
*   Ajuste de referências em arquivos de projeto (`.csproj`) e solução (`.sln`).

### 2026-05-09: Estruturação Inicial de Microserviços
*   Separação das responsabilidades de Autenticação em um microserviço dedicado.
*   Configuração inicial do ambiente Docker com suporte a réplicas de leitura no Postgres e cluster ClickHouse.
*   Implementação do login básico e suporte inicial a SSO Google.
