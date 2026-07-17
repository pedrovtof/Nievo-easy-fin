# Diagram

```mermaid
%% Use Case Diagram for Login Service (caso_uso_login)
flowchart LR
    Usuario[Usuário] -->|Login request| AuthService[Serviço de Login]
    AuthService -->|Validate credentials| Usuario
    AuthService -->|Generate token| Usuario
    AuthService -->|Internal processing| AuthService
```

> *Nota*: substitua o placeholder pelos detalhes reais quando disponíveis.
