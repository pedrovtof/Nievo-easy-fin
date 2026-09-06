# APIs e Integrações

A comunicação entre os clientes (Frontend React/Vite, aplicativos móbiles ou integrações externas) e os microsserviços do **Nievo EasyFin** é inteiramente padronizada e centralizada pelo **Kong API Gateway**.

---

## 🌐 1. Base URLs e Roteamento

Todas as chamadas devem utilizar o endereço público do Kong Gateway (Porta `8000`). O Gateway intercepta os prefixos de URL e roteia internamente para os respectivos serviços:

```
https://api.nievo.com.br/
├── /api/auth/...   --> Roteado para Auth Service (Porta 8081)
├── /api/core/...   --> Roteado para Core Service (Porta 8082)
└── /api/data/...   --> Roteado para Data Service Python (Porta 8083)
```

---

## 📑 2. Convenções e Headers Obrigatórios

### 📥 Headers HTTP

Todas as requisições enviadas à API devem incluir os seguintes cabeçalhos conforme a natureza do endpoint:

| Header | Tipo | Obrigatório em | Descrição |
| :--- | :--- | :--- | :--- |
| `Content-Type` | `string` | POST / PUT / PATCH | Deve ser `application/json`. |
| `User-Agent` | `string` | Cadastro (`/singup`) | Identificação do navegador/cliente para auditoria de termos. |
| `Host` | `string` | Cadastro (`/singup`) | Endereço IP / Host de origem para auditoria de termos. |
| `Authorization`| `string` | Endpoints Privados | Token no formato `Bearer <token_jwt>`. |

---

## 📦 3. Envelopes de Resposta Padronizados

Para garantir a previsibilidade na integração do frontend, todos os endpoints retornam um formato de envelope JSON consistente:

### 🟢 Resposta de Sucesso Padrão (`ResponseApiSucess`)

```json
{
  "sucess": true,
  "data": {
    "message": "Operação realizada com sucesso."
  }
}
```

### 🔴 Resposta de Erro Padrão (`ResponseApiError`)

```json
{
  "sucess": false,
  "errors": [
    "O campo 'email' é obrigatório.",
    "A senha deve conter no mínimo 8 caracteres e um símbolo especial."
  ]
}
```

### 📄 Resposta Paginada Padrão (`ResponsePaginationBase<T>`)

```json
{
  "sucess": true,
  "data": {
    "page": 1,
    "page_size": 10,
    "total_records": 42,
    "total_pages": 5,
    "items": [
      { "id": 1, "name": "Itaú" },
      { "id": 2, "name": "Nubank" }
    ]
  }
}
```

---

## 📑 4. Especificações de Endpoints

Consulte as seções detalhadas a seguir para verificar os contratos completos, payloads de entrada, parâmetros de consulta e códigos de retorno de cada microsserviço:

- **[Endpoints de Autenticação (Auth)](auth_endpoints.md):** Rotas do microsserviço `NievoEasyFin.Auth`.
- **[Endpoints de Contas e Cartões (Core)](core_endpoints.md):** Rotas do Monólito `NievoEasyFin.Core`.