# FinanceControl API

A RESTful API for personal finance management built with C# and ASP.NET Core .NET 8.

---

## Features

- User registration and authentication with JWT
- Income and expense tracking
- Category management (income/expense categories)
- Financial summary with total income, expenses and balance
- Secure endpoints — all routes require authentication except register/login
- Swagger UI with JWT support for easy testing

---

## Tech Stack

- **.NET 8 / ASP.NET Core** — Web API framework
- **Entity Framework Core** — ORM for database access
- **SQL Server** — Relational database
- **JWT Bearer Authentication** — Stateless auth
- **BCrypt** — Password hashing
- **Swagger / Swashbuckle** — API documentation

---

## Project Structure

```
FinanceControl/
├── Controllers/        # API endpoints
├── Services/           # Business logic
├── Repositories/       # Data access layer
├── Models/             # Database entities
├── DTOs/               # Request/response objects
├── Data/               # DbContext
└── Migrations/         # EF Core migrations
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or Docker)

### Installation

1. Clone the repository
```bash
git clone https://github.com/GabrielStrehle/FinanceControl-API.git
cd FinanceControl-API
```

2. Update the connection string in `appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=FinanceControlDb;Trusted_Connection=True;"
}
```

3. Update the JWT settings in `appsettings.json`
```json
"Jwt": {
  "Key": "your-secret-key-here",
  "Issuer": "FinanceControlAPI",
  "Audience": "FinanceControlAPI",
  "ExpiresInHours": "8"
}
```

4. Run the database migrations
```bash
dotnet ef database update
```

5. Start the application
```bash
dotnet run
```

6. Open Swagger UI at `https://localhost:{port}/`

---

## API Endpoints

### Auth
| Method | Route | Description | Auth required |
|--------|-------|-------------|--------------|
| POST | `/api/auth/register` | Register a new user | No |
| POST | `/api/auth/login` | Login and get JWT token | No |

### Categories
| Method | Route | Description | Auth required |
|--------|-------|-------------|--------------|
| GET | `/api/categories` | List all categories | Yes |
| POST | `/api/categories` | Create a category | Yes |
| PUT | `/api/categories/{id}` | Update a category | Yes |
| DELETE | `/api/categories/{id}` | Delete a category | Yes |

### Transactions
| Method | Route | Description | Auth required |
|--------|-------|-------------|--------------|
| GET | `/api/transactions` | List all transactions | Yes |
| GET | `/api/transactions/{id}` | Get transaction by ID | Yes |
| GET | `/api/transactions/summary` | Get financial summary | Yes |
| POST | `/api/transactions` | Create a transaction | Yes |
| PUT | `/api/transactions/{id}` | Update a transaction | Yes |
| DELETE | `/api/transactions/{id}` | Delete a transaction | Yes |

---

## Example Requests

### Register
```json
POST /api/auth/register
{
  "name": "Gabriel",
  "email": "gabriel@email.com",
  "password": "123456"
}
```

### Create Transaction
```json
POST /api/transactions
Authorization: Bearer {token}

{
  "description": "Salary",
  "amount": 3000.00,
  "date": "2026-03-15T00:00:00",
  "type": "income",
  "categoryId": 1
}
```

### Financial Summary Response
```json
GET /api/transactions/summary

{
  "totalIncome": 3000.00,
  "totalExpense": 850.00,
  "balance": 2150.00,
  "totalTransactions": 5
}
```

---

## Author

**Gabriel Strehle**
- LinkedIn: [linkedin.com/in/gabrielstrehle](https://linkedin.com/in/gabrielstrehle)
- GitHub: [github.com/GabrielStrehle](https://github.com/GabrielStrehle)