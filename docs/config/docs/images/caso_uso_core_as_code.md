# Diagram

```mermaid
%% Use Case Diagram for Core Service (caso_uso_core)
flowchart LR
    Cliente[Cliente] -->|Ação 1| CoreService[Serviço Core]
    CoreService -->|Ação 2| Cliente
    CoreService -->|Ação interna| CoreService
```

> *Nota*: substitua o placeholder pelos detalhes reais quando disponíveis.
