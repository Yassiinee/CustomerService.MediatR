# CustomerService.MediatR

A clean, production-ready **ASP.NET Core Web API** demonstrating **DDD (Domain-Driven Design)**,
**CQRS**, **MediatR**, and **FluentValidation** best practices with **JWT Authentication**.

This project is designed as a **reference architecture** for building scalable, testable, and maintainable backend services.

---

## 🧱 Architecture Overview

The solution follows **Clean Architecture** principles:

```
CustomerService.MediatR
│
├─ MediatRAPI                → API layer (HTTP, Controllers, Swagger)
├─ MediatRHandlers           → Application, Domain & Infrastructure
└─ CustomerService.MediatR.sln
```

### Layer Responsibilities

| Layer | Responsibility |
|-----|---------------|
| API | HTTP endpoints, request/response mapping, authentication |
| Application | CQRS, MediatR requests & handlers |
| Domain | Business entities & rules |
| Infrastructure | Persistence, external services |

---

## 🧠 Key Concepts

### ✔ MediatR
- Decouples controllers from business logic
- Implements **Command / Query** separation
- Improves testability and maintainability

### ✔ CQRS
- **Commands** → change state (e.g., CreateCustomerCommand)
- **Queries** → read data (e.g., GetCustomerByIdQuery)
- Clear separation of responsibilities

### ✔ DDD
- Business logic lives in the **Domain**
- Infrastructure details are abstracted
- Strong boundaries between layers

### ✔ FluentValidation
- Validates commands & queries
- Executed automatically via MediatR pipeline
- No validation logic in controllers

### ✔ JWT Authentication
- Secure token-based authentication
- Protected customer endpoints
- Token generation via `/api/auth/token` endpoint

---

## 📁 Folder Structure

```
MediatRHandlers
 ├─ Application
 │   ├─ Customers
 │   │   ├─ Commands
 │   │   ├─ Queries
 │   │   ├─ Dtos
 │   │   └─ Validators
 │   ├─ Common
 │   │   ├─ Interfaces
 │   │   ├─ Behaviors
 │   │   └─ Exceptions
 │
 ├─ Domain
 │   └─ Entities
 │
 └─ Infrastructure
     ├─ Persistence
     └─ DependencyInjection.cs
```

---

## 🔄 Request Flow

```
HTTP Request
   ↓
Global Exception Middleware
   ↓
CORS Policy
   ↓
JWT Authentication
   ↓
Controller
   ↓
IMediator.Send()
   ↓
Validation Pipeline (FluentValidation)
   ↓
Request Handler
   ↓
Domain / Repository
   ↓
Response
```

---

## 🚀 Getting Started

### Prerequisites
- .NET 10 or later
- Visual Studio / VS Code / Rider

### Run the API

```bash
dotnet restore
dotnet build
dotnet run --project MediatRAPI
```

### Swagger UI

```
https://localhost:{port}/swagger
```

### Authentication

1. **Get JWT Token** (Development):
   ```bash
   POST /api/auth/token
   ```
   
2. **Use Token** in subsequent requests:
   ```bash
   Authorization: Bearer {your-token}
   ```

3. **Access Protected Endpoints**:
   ```bash
   POST /api/v1/customers
   GET /api/v1/customers/{id}
   ```

---

## 📋 API Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/token` | Generate JWT token | No |
| POST | `/api/v1/customers` | Create customer | Yes |
| GET | `/api/v1/customers/{id}` | Get customer by ID | Yes |
| GET | `/health` | Health check | No |

---

## 🔧 Configuration

### JWT Settings (`appsettings.Development.json`)

```json
{
  "Jwt": {
    "Issuer": "CustomerService.MediatR",
    "Audience": "CustomerService.MediatR.Client",
    "Key": "your-secret-key-here"
  }
}
```

### CORS Policy
- **Development**: Allows any origin
- **Production**: Configurable via `AllowedOrigins` setting

---

## 🔧 Dependency Injection

All services are registered via a single entry point:

```csharp
builder.Services.AddApplication();
```

This ensures:
- Loose coupling
- Easy testing
- Infrastructure independence

---

## 🏗 Production-Ready Features

✔ Clean Architecture  
✔ MediatR pipelines  
✔ FluentValidation  
✔ **Global exception middleware**  
✔ **Serilog structured logging**  
✔ **API versioning** (v1.0)  
✔ **JWT Authentication & Authorization**  
✔ **CORS configuration**  
✔ **Health checks** (`/health`)  
✔ Swagger documentation with versioning  
✔ Scalable folder structure  
✔ Test-friendly design  

---

## 🔮 Planned Enhancements

- EF Core + migrations
- Domain events
- Caching (Redis)
- Unit & integration tests
- Role-based authorization
- Rate limiting
- Database persistence layer

---

## 📜 License

MIT License
