# Arquitetura e Componentes do Frontend

O cliente web do **Nievo EasyFin** é uma Single Page Application (SPA) moderna, acessível e responsiva construída em **React 18** com o bundler de alta performance **Vite** e estilização visual com **Material UI (MUI v6)**.

---

## 🚀 1. Tecnologias e Decisões de Arquitetura

- **React 18 + Vite 6:** Escolhido pela inicialização instantânea do servidor de desenvolvimento (HMR), compilação ultrarrápida e footprint reduzido no bundle final de produção.
- **Material UI (MUI v6):** Utilizado como biblioteca padronizada de componentes e design system, garantindo acessibilidade nativa (ARIA), temas consistente (Light e Dark Mode) e componentes dinâmicos (Grid, DataGrid, Cards, Modals).
- **React Router DOM (v6):** Gerenciamento de rotas do lado do cliente com suporte a rotas públicas, rotas protegidas e redirecionamentos automáticos.

---

## 📁 2. Padrão de Estrutura de Arquivos Modular

Cada página e componente modular no diretório `frontend/web/src/` segue rigorosamente a separação de responsabilidades em 4 arquivos padronizados:

```
src/pages/Dashboard/
├── index.jsx      # Exportação principal e injeção de estado/contexto
├── View.jsx       # Estrutura visual JSX e componentes MUI
├── styles.js      # Estilização customizada (MUI styled / sx props)
└── api.js         # Chamadas HTTP REST para o backend (via Axios / Fetch)
```

---

## 🛡️ 3. Gerenciamento de Rotas e Proteção de Sessão (`App.jsx`)

As rotas são classificadas entre **Rotas Públicas** e **Rotas Protegidas**:

```mermaid
flowchart TD
    RequestRoute["Navegação para URL"] --> GuardCheck{"Tipo de Rota"}
    
    GuardCheck -->|Pública (ex: /login)| PublicGuard{"Usuário Autenticado?"}
    PublicGuard -->|Sim| RedirDash["Redireciona para Dashboard (/)"]
    PublicGuard -->|Não| RenderPubPage["Renderiza Página Pública"]

    GuardCheck -->|Protegida (ex: /transactions)| PrivateGuard{"Possui Token JWT Válido no localStorage?"}
    PrivateGuard -->|Não| RedirLogin["Redireciona para /login"]
    PrivateGuard -->|Sim| RenderLayout["Renderiza Layout (Sidebar + Header + Conteúdo)"]
```

### 🗺️ Tabela de Rotas da Aplicação:

| Rota | Componente | Tipo | Proteção | Descrição |
| :--- | :--- | :--- | :--- | :--- |
| `/login` | `Login` | Pública | `PublicRoute` | Formulário de autenticação por e-mail/senha e login SSO Google. |
| `/register` | `Register` | Pública | `PublicRoute` | Formulário de criação de conta com validação de termos. |
| `/forgot-password`| `ForgotPassword`| Pública | `PublicRoute` | Solicitação e confirmação de PIN de redefinição de senha. |
| `/confirm-email` | `ConfirmEmail` | Pública | `PublicRoute` | Validação de PIN de ativação de conta. |
| `/terms` | `Terms` | Pública | Livre | Exibição pública dos Termos de Uso vigentes no sistema. |
| `/` | `Dashboard` | Protegida| `ProtectedRoute` | Visão geral de saldo, despesas mensais e gráficos analíticos. |
| `/transactions` | `Transactions` | Protegida| `ProtectedRoute` | Listagem e lançamento de receitas e despesas. |
| `/budget` | `Budget` | Protegida| `ProtectedRoute` | Definição e acompanhamento de limites de gastos por categoria. |
| `/settings` | `Settings` | Protegida| `ProtectedRoute` | Configurações da conta do usuário e preferências visuais. |

---

## 🎨 4. Tema e Suporte a Dados Simulados (`mockData`)

- **Contexto de Tema (`ThemeContext.jsx`):** Permite a alternância dinâmica entre modo claro (Light Mode) e modo escuro (Dark Mode).
- **Mapeamento de Textos (`locales/texts.json`):** Centralização das strings e mensagens do sistema para suporte facilitado a i18n / internacionalização.
- **Modo Demonstrativo (`MockGuidePopup`):** Para fins acadêmicos ou testes offline, a aplicação inclui um mecanismo de dados simulados (`src/services/mockData.js`) acionado via aviso interativo.
