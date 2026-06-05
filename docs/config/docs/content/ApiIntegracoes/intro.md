# API e Integrações

A comunicação no Nievo Easy Fin é centralizada através de um API Gateway, garantindo segurança e padronização.

## Gateway: Kong

O **Kong** atua como o cérebro da rede, interceptando todas as chamadas externas. Ele é responsável por:
*   **Rate Limiting:** Protege os serviços contra excesso de requisições.
*   **Auth Centralizada:** Valida tokens JWT antes mesmo da requisição chegar aos microserviços.
*   **Abstração de Endereços:** O frontend conhece apenas o endereço do Kong, que por sua vez conhece a topologia interna do Kubernetes.

## Estrutura de Endpoints

Os endpoints são categorizados por visibilidade e versão:

### Endpoints Públicos (`/api/public/v1/...`)
Destinados ao fluxo de acesso inicial, onde o usuário ainda não possui uma sessão ativa ou precisa realizar ações de recuperação.
*   **Autenticação:** Login tradicional e SSO (Google, etc).
*   **Gestão de Senha:** Solicitação de reset e atualização via token.
*   **Cadastro de Usuário (`POST /singup`):** Cria um novo usuário com e-mail e senha.
    *   **Headers obrigatórios:** `User-Agent`, `Host`
    *   **Body obrigatório:** `name`, `email`, `password`, `accept_terms: true`
    *   **Respostas:** `201 Created` (sucesso) | `400 Bad Request` (validação, e-mail duplicado, termos não aceitos, erro ao registrar aceite)
*   **Cadastro de Usuário SSO (`POST /singup-sso`):** Cria ou vincula um usuário via provedor SSO.
    *   **Headers obrigatórios:** `User-Agent`, `Host`
    *   **Body obrigatório:** `provider_name`, `provider_access_token`, `accept_terms: true`
    *   **Respostas:** `201 Created` (novo usuário) | `200 OK` (usuário já existe) | `400 Bad Request` (provedor inválido, token inválido, termos não aceitos)

### Endpoints Privados (`/api/private/v1/...`)
Exigem um token JWT válido. O Kong valida a assinatura do token antes de encaminhar a requisição.
*   **Core Business:** Gestão de transações, categorias e orçamentos.
*   **Perfil:** Dados do usuário e configurações de conta.

## Contratos de Dados (D TOs)
A aplicação utiliza DTOs rigorosos para garantir que apenas os dados necessários sejam trafegados, minimizando o payload e protegendo informações sensíveis do banco de dados.