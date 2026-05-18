# Atualizações e Histórico

Este documento registra as principais milestones e alterações arquiteturais do projeto Nievo Easy Fin.

## Versão Atual: Alpha (Em desenvolvimento)

O projeto encontra-se em fase ativa de desenvolvimento, com as funcionalidades base de autenticação e core sendo consolidadas.

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

## Próximos Passos
*   Finalização da integração entre o Monólito Core e o Microserviço de Analytics (Python).
*   Expansão da cobertura de testes unitários e de integração.
*   Implementação de dashboards avançados no frontend utilizando os insights gerados pelo microserviço de dados.