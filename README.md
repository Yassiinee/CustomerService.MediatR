# CustomerService.MediatR

A clean, production-ready **ASP.NET Core Web API** demonstrating **DDD (Domain-Driven Design)**,
**CQRS**, **MediatR**, and **FluentValidation** best practices with **JWT Authentication** and **comprehensive testing**.

This project is designed as a **reference architecture** for building scalable, testable, and maintainable backend services.

---

## 🧱 Architecture Overview

The solution follows **Clean Architecture** principles with a comprehensive test suite:

```
CustomerService.MediatR/
│
├─ MediatRAPI                    → API layer (Controllers, Middleware, Configuration)
├─ MediatRHandlers              → Application, Domain & Infrastructure
├─ MediatRAPI.UnitTests         → Isolated unit tests with mocked dependencies
├─ MediatRAPI.IntegrationTests  → End-to-end API testing with real HTTP requests
└─ CustomerService.MediatR.sln
```

### Layer Responsibilities

| Layer | Responsibility | Test Coverage |
|-------|----------------|---------------|
| **API** | HTTP endpoints, middleware, authentication | Integration Tests |
| **Application** | CQRS handlers, validation, behaviors | Unit + Integration |
| **Domain** | Business entities & rules | Unit Tests |
| **Infrastructure** | Persistence, external services | Unit Tests |

---

## 🧠 Key Concepts

### ✔ MediatR
- Decouples controllers from business logic
- Implements **Command / Query** separation
- Improves testability and maintainability
- Pipeline behaviors for cross-cutting concerns

### ✔ CQRS
- **Commands** → change state (e.g., CreateCustomerCommand)
- **Queries** → read data (e.g., GetCustomerByIdQuery)
- Clear separation of responsibilities
- Optimized read/write models

### ✔ DDD
- Business logic lives in the **Domain**
- Infrastructure details are abstracted
- Strong boundaries between layers
- Domain entities with encapsulated behavior

### ✔ FluentValidation
- Validates commands & queries
- Executed automatically via MediatR pipeline
- Comprehensive validation rules
- Clear, readable validation logic

### ✔ JWT Authentication
- Secure token-based authentication
- Protected customer endpoints
- Token generation via `/api/auth/token` endpoint
- Claims-based authorization

### ✔ Comprehensive Testing
- **Unit Tests**: Fast, isolated component testing
- **Integration Tests**: End-to-end API testing
- **High Code Coverage**: 95%+ across all layers
- **Test-Driven Development** patterns

---

## 📁 Folder Structure

```
MediatRHandlers/
 ├─ Application/
 │   ├─ Customers/
 │   │   ├─ Commands/         (CreateCustomerCommand + Handler + Validator)
 │   │   ├─ Queries/          (GetCustomerByIdQuery + Handler)
 │   │   └─ Dtos/            (CustomerDto)
 │   ├─ Common/
 │   │   ├─ Interfaces/       (ICustomerRepository)
 │   │   ├─ Behaviors/        (ValidationBehavior, LoggingBehavior)
 │   │   └─ Exceptions/       (NotFoundException)
 │
 ├─ Domain/
 │   └─ Entities/            (Customer)
 │
 └─ Infrastructure/
     ├─ Persistence/          (InMemoryCustomerRepository)
     └─ DependencyInjection.cs

MediatRAPI.UnitTests/
 ├─ Application/             (Handler, Validator, Behavior tests)
 ├─ Domain/                  (Entity tests)
 └─ TestUtilities/           (Test data builders)

MediatRAPI.IntegrationTests/
 ├─ Controllers/             (API endpoint tests)
 ├─ Infrastructure/          (Health checks, versioning)
 ├─ Middleware/              (Exception handling)
 └─ Common/                  (Test infrastructure)
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
Logging Behavior
   ↓
Validation Behavior (FluentValidation)
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
- **.NET 10.0** or later
- Visual Studio 2024 / VS Code / Rider

### Clone and Run

```bash
# Clone the repository
git clone https://github.com/Yassiinee/CustomerService.MediatR.git
cd CustomerService.MediatR

# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run the API
dotnet run --project MediatRAPI

# Run all tests
dotnet test
```

### Swagger UI

Navigate to: `https://localhost:{port}/swagger`

### Authentication Flow

1. **Generate JWT Token**:
   ```bash
   POST /api/auth/token
   Content-Type: application/json
   
   {
     "username": "testuser",
     "password": "password"
   }
   ```

2. **Use Token in Requests**:
   ```bash
   Authorization: Bearer {your-jwt-token}
   ```

3. **Access Protected Endpoints**:
   ```bash
   POST /api/v1.0/customers
   GET /api/v1.0/customers/{id}
   ```

---

## 📋 API Endpoints

| Method | Endpoint | Description | Auth Required | Test Coverage |
|--------|----------|-------------|---------------|---------------|
| POST | `/api/auth/token` | Generate JWT token | No | ✅ Integration |
| POST | `/api/v1.0/customers` | Create customer | Yes | ✅ Unit + Integration |
| GET | `/api/v1.0/customers/{id}` | Get customer by ID | Yes | ✅ Unit + Integration |
| GET | `/health` | Health check | No | ✅ Integration |

---

## 🧪 Testing Strategy

### Test Projects

| Project | Purpose | Test Count | Coverage |
|---------|---------|------------|----------|
| **MediatRAPI.UnitTests** | Isolated component testing | 15+ tests | 95%+ |
| **MediatRAPI.IntegrationTests** | End-to-end API testing | 12+ tests | 90%+ |

### Unit Tests
- ✅ Domain entity behavior
- ✅ Application handlers with mocked dependencies  
- ✅ Validation rules and error scenarios
- ✅ MediatR pipeline behaviors
- ✅ Exception handling

### Integration Tests
- ✅ Complete HTTP request/response flows
- ✅ Authentication and authorization
- ✅ Global exception middleware
- ✅ API versioning and health checks
- ✅ Real validation pipeline execution

### Running Tests

```bash
# Run all tests
dotnet test

# Run only unit tests
dotnet test MediatRAPI.UnitTests

# Run only integration tests  
dotnet test MediatRAPI.IntegrationTests

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

---

## 🔧 Configuration

### JWT Settings (`appsettings.Development.json`)

```json
{
  "Jwt": {
    "Issuer": "CustomerService.MediatR",
    "Audience": "CustomerService.MediatR.Client", 
    "Key": "YourSuperSecretKeyForJWTTokenGeneration123!",
    "ExpiryMinutes": 60
  }
}
```

### CORS Policy
- **Development**: Allows any origin (`AllowAnyOrigin()`)
- **Production**: Configure `AllowedOrigins` in appsettings

### Logging Configuration
- **Serilog** structured logging
- **Console** and **File** sinks
- **Request/Response** logging via LoggingBehavior

---

## 🏗 Production-Ready Features

### Core Architecture
✔ **Clean Architecture** with clear layer separation  
✔ **CQRS** with MediatR implementation  
✔ **DDD** principles and domain modeling  
✔ **Dependency Injection** with service registration  

### Cross-Cutting Concerns
✔ **FluentValidation** with pipeline behavior  
✔ **Global exception middleware** with structured errors  
✔ **Serilog structured logging** with request correlation  
✔ **JWT Authentication & Authorization**  
✔ **CORS configuration** for cross-origin requests  

### API Features
✔ **API versioning** (v1.0) with URL path versioning  
✔ **Swagger documentation** with JWT authorization  
✔ **Health checks** endpoint (`/health`)  
✔ **Request/Response validation**  

### Quality & Testing
✔ **Comprehensive unit tests** (95%+ coverage)  
✔ **End-to-end integration tests** with real HTTP  
✔ **Test-driven development** patterns  
✔ **Mocked dependencies** for isolated testing  

### DevOps Ready
✔ **Dockerizable** application structure  
✔ **Environment-specific** configuration  
✔ **Structured logging** for observability  
✔ **Health check** endpoints for monitoring  

---

## 🔮 Planned Enhancements

### Persistence Layer
- [ ] **EF Core** with SQL Server/PostgreSQL
- [ ] **Database migrations** and seeding
- [ ] **Repository pattern** with real persistence
- [ ] **Unit of Work** pattern implementation

### Advanced Features  
- [ ] **Domain events** with event handlers
- [ ] **Caching layer** (Redis/In-Memory)
- [ ] **Rate limiting** middleware
- [ ] **Role-based authorization** with claims

### Observability
- [ ] **OpenTelemetry** distributed tracing
- [ ] **Prometheus** metrics collection
- [ ] **Application Insights** integration
- [ ] **Structured logging** enhancements

### Testing Enhancements
- [ ] **Architecture tests** (NetArchTest)
- [ ] **Performance tests** (NBomber)
- [ ] **Contract testing** (Pact)
- [ ] **Mutation testing** (Stryker.NET)

---

## 📚 Documentation

- [**Unit Tests Documentation**](MediatRAPI.UnitTests/README.md) - Detailed unit testing guide
- [**Integration Tests Documentation**](MediatRAPI.IntegrationTests/README.md) - API testing guide
- [**Test Strategy Documentation**](TEST_README.md) - Overall testing approach

---

## 🤝 Contributing

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Write tests** for your changes (unit + integration)
4. **Commit** your changes (`git commit -m 'Add amazing feature'`)
5. **Push** to the branch (`git push origin feature/amazing-feature`)
6. **Open** a Pull Request

### Development Guidelines
- Follow **Clean Architecture** principles
- Maintain **high test coverage** (95%+)
- Use **conventional commit** messages
- Update **documentation** for new features

---

## 📜 License

MIT License - see the [LICENSE](LICENSE) file for details

---

## 🏆 Acknowledgments

- **Clean Architecture** by Robert C. Martin
- **MediatR** library by Jimmy Bogard  
- **FluentValidation** by Jeremy Skinner
- **.NET Community** for excellent tooling and patterns
