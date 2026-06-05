# Fluxos de Negócio

Os fluxos de negócio do Nievo Easy Fin foram desenhados para serem intuitivos, seguros e eficientes, garantindo que o usuário tenha total controle sobre sua vida financeira com o mínimo de fricção.

## 1. Fluxo de Integração (Onboarding)

O primeiro contato do usuário com a plataforma ocorre através do registro.

*   **Registro Tradicional:** O usuário fornece Nome, Email, Senha e deve **aceitar os Termos de Uso** (`accept_terms: true`). O sistema valida a complexidade da senha, a unicidade do e-mail e o aceite dos termos antes de criar a conta. Os headers `Host` e `User-Agent` são obrigatórios e ficam registrados junto ao aceite de termos para fins de auditoria e compliance.
*   **Registro via SSO (Google):** O usuário pode optar por utilizar sua conta Google. O sistema valida o token enviado pelo frontend, recupera os dados básicos e vincula a conta. O aceite dos Termos de Uso (`accept_terms: true`) também é **obrigatório** neste fluxo, e os headers `Host` e `User-Agent` são igualmente registrados.
*   **Registro de Aceite de Termos:** Para cada cadastro bem-sucedido, o sistema persiste um registro na tabela `journey.users_accepted_terms`, associando o usuário ao termo vigente (`journey.accept_terms`) no momento do cadastro, incluindo a versão do termo, data, IP de origem (via Host) e agente do navegador.

## 2. Fluxo de Autenticação e Segurança

A segurança é centralizada no serviço de **Auth**.

*   **Login:** Após a validação das credenciais, o sistema gera um token **JWT (JSON Web Token)** assinado. Este token deve ser enviado em todas as requisições subsequentes para endpoints privados.
*   **Recuperação de Senha:** Caso o usuário esqueça sua senha, ele pode solicitar um reset. O sistema gera um PIN temporário, armazena em cache (Redis) e envia por e-mail (via SMTP). O usuário utiliza este PIN para definir uma nova senha.

## 3. Gestão Financeira (Core)

Uma vez autenticado, o usuário interage com o **Monólito Core**.

*   **Lançamentos:** Registro de despesas e receitas. Cada lançamento pode ser categorizado e associado a uma conta específica.
*   **Categorização:** O sistema permite a criação de categorias e subcategorias personalizadas para um detalhamento preciso dos gastos.
*   **Metas:** Definição de orçamentos mensais por categoria. O sistema monitora o consumo em tempo real e alerta o usuário sobre o progresso das metas.

## 4. Análise e Insights (Analytics)

O processamento pesado de dados ocorre de forma assíncrona ou dedicada.

*   **Processamento de Dados:** Os dados brutos do Postgres são refletidos no **ClickHouse** para análise de alta performance.
*   **Geração de Insights:** O microserviço em Python processa os volumes históricos para identificar padrões de consumo e fornecer previsões financeiras baseadas em comportamento passado.

