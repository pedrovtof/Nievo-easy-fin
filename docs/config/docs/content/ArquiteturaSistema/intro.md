# Arquitetura do Sistema

O Nievo Easy Fin utiliza uma **arquitetura híbrida**, combinando um monólito robusto com microserviços especializados para otimizar desempenho e escalabilidade onde é mais necessário.

## Componentes Principais

### 1. Core Monolith (C# / .NET)
O coração da aplicação, responsável pelas regras de negócio principais, orquestração de dados e serviços fundamentais. Foi escolhido pela sua consistência, resiliência e forte tipagem.

### 2. Microserviço de Autenticação (C# / .NET)
Um serviço isolado dedicado à gestão de identidade, login (incluindo SSO) e segurança. Este isolamento garante que a superfície de ataque seja minimizada e que o serviço possa ser escalado independentemente.

### 3. Microserviço de Dados e Análise (Python / Flask)
Especializado em operações matemáticas complexas e processamento analítico. O Python foi escolhido para este componente devido ao seu ecossistema maduro de bibliotecas de ciência de dados e performance em cálculos.

## Estratégia de Persistência (Polyglot Persistence)

A aplicação utiliza diferentes tecnologias de banco de dados para atender a requisitos específicos:

*   **PostgreSQL:** Banco relacional principal para dados transacionais e consistência forte. Utiliza uma arquitetura com réplicas para leitura para garantir alta disponibilidade.
*   **Redis:** Utilizado como camada de cache e para suporte ao API Gateway (Kong), proporcionando tempos de resposta extremamente rápidos para dados acessados frequentemente.
*   **ClickHouse:** Banco de dados colunar focado em análise de grandes volumes de dados (OLAP), permitindo consultas analíticas rápidas sobre o histórico de gastos.

## Infraestrutura e Gateway

A solução é containerizada utilizando **Docker** e orquestrada via **Kubernetes (K8S)**. 

O **Kong API Gateway** atua como o ponto de entrada único para o ecossistema, gerenciando:
*   Roteamento de tráfego.
*   Rate Limiting para proteção contra abusos.
*   Autenticação centralizada.