# Order System – API + Frontend

Projeto completo contendo backend em .NET 8 e frontend em Angular 18 para gerenciamento de pedidos, produtos e clientes.

---

## 📦 Funcionalidades

### 👥 Clientes
- Criar cliente
- Listar clientes

### 📦 Produtos
- Criar produto
- Listar produtos

### 🧾 Pedidos
- Criar pedido (com quantidades!)
- Listar pedidos
- Cancelar pedido
- Marcar pedido como pago
- Ver total do pedido
- Histórico interno de eventos

### 🔖 Status dos Pedidos
- **0 – Falta pagar**
- **1 – Pago**
- **2 – Cancelado**

---

## 📁 Estrutura do Projeto

```
ApiPedidos/
│── OrderSystemApi/               # Backend .NET
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   │   └── Interfaces/
│   ├── OrderSystemApi.csproj
│   └── OrderSystemApi.sln
│
│── OrderSystemApi.Tests/         # Testes xUnit
│   └── *.cs
│
└── order-system-frontend/        # Frontend Angular 18
    ├── src/
    │   └── app/
    │       └── components/
    ├── package.json
    └── angular.json
```

---

## 🧩 Como Rodar o Backend (.NET API)

Acesse o diretório:

```
ApiPedidos/OrderSystemApi/
```

Execute:

```
dotnet restore
dotnet build
dotnet run
```

A API rodará em:

➡️ **http://localhost:5000**

Swagger:

➡️ **http://localhost:5000/swagger**

---

## 🌐 Como Rodar o Frontend (Angular)

Acesse o diretório:

```
ApiPedidos/order-system-frontend/
```

Instale dependências:

```
npm install
```

Rode o projeto:

```
npm start
```

O frontend abrirá em:

➡️ **http://localhost:4200**

---

## ✔️ Requisitos Atendidos

- CRUD de clientes
- CRUD de produtos
- Criação de pedidos com **quantidade por produto**
- Cancelamento e pagamento de pedidos
- Validação completa no backend
- Histórico de ações
- API documentada com Swagger
- Testes unitários reais com xUnit
- Frontend funcional consumindo a API

---

## 👨‍💻 Autor
Guilherme Aparecido
