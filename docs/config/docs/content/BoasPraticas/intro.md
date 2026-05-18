# Boas Práticas e Padrões Técnicos

Para manter a consistência e a qualidade do código no Nievo Easy Fin, seguimos um conjunto de diretrizes e padrões de desenvolvimento.

## 1. Arquitetura e Organização de Código

### Camada de Aplicação (Application Layer)
*   **Interfaces:** Todos os serviços devem ser expostos através de interfaces para facilitar a inversão de controle e testes.
*   **DTOs (Data Transfer Objects):** Requisições e respostas devem utilizar DTOs específicos. Nunca exponha entidades de banco de dados diretamente nos controllers.
*   **Validators:** Utilizamos `FluentValidation` para garantir a integridade dos dados de entrada. As validações devem ser assíncronas quando necessário.

### Camada de Domínio (Domain Layer)
*   **Entidades:** Devem representar fielmente o modelo de negócio e as restrições do banco de dados.
*   **Helpers:** Lógicas reutilizáveis e utilitárias devem ser centralizadas em classes Helper.

## 2. Segurança

*   **Senhas:** Nunca armazene senhas em texto plano. Utilizamos PBKDF2 com múltiplas iterações e salting para garantir a segurança dos hashes.
*   **Autenticação:** Baseada em tokens JWT. O segredo do token deve ser mantido em variável de ambiente e nunca commitado.
*   **Autorização:** Endpoints sensíveis devem utilizar o atributo `[Authorize]` e validar as claims do usuário.

## 3. Documentação de Código

*   **XML Docstrings:** Todo método público em interfaces e serviços deve possuir documentação XML detalhando sua finalidade, parâmetros, retornos e possíveis exceções.
*   **README:** Mantenha o README atualizado com a visão geral do projeto e instruções de execução.

## 4. Banco de Dados

*   **Migrations:** Mudanças no esquema de banco de dados devem ser feitas exclusivamente via migrations.
*   **Consultas:** Utilize consultas assíncronas (`async/await`) para evitar o bloqueio de threads de execução.
*   **Nomenclatura:** Tabelas e colunas devem seguir o padrão `snake_case` (para compatibilidade com Python/Postgres) ou `PascalCase` conforme o contexto do serviço.

## 5. Testes

*   **Testes Unitários:** Todo novo serviço ou lógica complexa deve ser acompanhado de testes unitários.
*   **Mocks:** Utilize frameworks de Mocking para isolar as dependências externas durante os testes.