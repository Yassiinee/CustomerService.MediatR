# CustomerService.MediatR

A clean, production-ready **ASP.NET Core Web API** demonstrating **DDD (Domain-Driven Design)**,
**CQRS**, **MediatR**, and **FluentValidation** best practices.

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
API | HTTP endpoints, request/response mapping |
Application | CQRS, MediatR requests & handlers |
Domain | Business entities & rules |
Infrastructure | Persistence, external services |

---

## 🧠 Key Concepts

### ✔ MediatR
- Decouples controllers from business logic
- Implements **Command / Query** separation
- Improves testability and maintainability

### ✔ CQRS
- **Commands** → change state
- **Queries** → read data
- Clear separation of responsibilities

### ✔ DDD
- Business logic lives in the **Domain**
- Infrastructure details are abstracted
- Strong boundaries between layers

### ✔ FluentValidation
- Validates commands & queries
- Executed automatically via MediatR pipeline
- No validation logic in controllers

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
✔ **API versioning**  
✔ Swagger documentation  
✔ Scalable folder structure  
✔ Test-friendly design  

---

## 🔮 Planned Enhancements

- EF Core + migrations
- Domain events
- Caching (Redis)
- Unit & integration tests
- Authentication & Authorization
- Rate limiting

---

## 📜 License

MIT License
