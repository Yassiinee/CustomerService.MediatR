# Customer Service MediatR

A .NET 10 Web API project demonstrating the MediatR pattern implementation for customer management operations.

## Overview

This solution showcases the use of the **MediatR** library to implement the Mediator pattern in a clean architecture approach. The project separates concerns between API endpoints and business logic handlers, promoting loose coupling and testability.

## Architecture

The solution consists of two main projects:

### 1. **MediatRHandlers** (Class Library)
Contains the core business logic and domain models:

- **Entities**: Domain models
  - `Customer.cs` - Customer entity with properties: FirstName, LastName, EmailAddress, Address

- **Requests**: MediatR request objects
  - `CreateCustomerRequest.cs` - Request to create a new customer (returns int)
  - `GetCustomerRequest.cs` - Request to retrieve a customer by ID

- **RequestHandlers**: MediatR request handlers
  - `CreateCustomerHandler.cs` - Handles customer creation logic
  - `GetCustomerHandler.cs` - Handles customer retrieval logic

- **Repositories**: Data access layer
  - `ICustomerRepository.cs` - Repository interface
  - `CustomerRepository.cs` - In-memory implementation using Dictionary<int, Customer>

- **MediatRDependencyHandler.cs**: Extension methods for dependency injection setup

### 2. **MediatRAPI** (ASP.NET Core Web API)
The REST API layer:

- **Controllers**
  - `CustomerController.cs` - Exposes HTTP endpoints for customer operations

- **Program.cs**: Application configuration and startup

## Features

- **MediatR Pattern**: Decouples request/response from handlers
- **Repository Pattern**: Abstracts data access logic
- **Dependency Injection**: Fully integrated with ASP.NET Core DI
- **Swagger/OpenAPI**: API documentation and testing interface
- **In-Memory Storage**: Simple dictionary-based storage for demo purposes

## API Endpoints

### Get Customer
```
GET /Customer/customerId?customerId={id}
```
Returns customer details for the specified ID.

### Create Customer
```
POST /Customer
```
Creates a new customer and returns the assigned customer ID.

**Request Body:**
```json
{
  "firstName": "string",
  "lastName": "string",
  "emailAddress": "string",
  "address": "string"
}
```

## Technology Stack

- **.NET 10**
- **ASP.NET Core Web API**
- **MediatR** - Mediator pattern implementation
- **Swashbuckle.AspNetCore** - Swagger/OpenAPI support
- **C# 14.0**

## Getting Started

### Prerequisites
- .NET 10 SDK

### Running the Application

1. Clone the repository
2. Navigate to the solution directory
3. Build the solution:
   ```bash
   dotnet build
   ```
4. Run the API:
   ```bash
   dotnet run --project MediatRAPI
   ```
5. Open Swagger UI in your browser (typically `https://localhost:5001/swagger` or `http://localhost:5000/swagger`)

## Project Structure

```
CustomerService.MediatR/
??? MediatRHandlers/
?   ??? Entities/
?   ?   ??? Customer.cs
?   ??? Requests/
?   ?   ??? CreateCustomerRequest.cs
?   ?   ??? GetCustomerRequest.cs
?   ??? RequestHandlers/
?   ?   ??? CreateCustomerHandler.cs
?   ?   ??? GetCustomerHandler.cs
?   ??? Repositories/
?   ?   ??? ICustomerRepository.cs
?   ?   ??? CustomerRepository.cs
?   ??? MediatRDependencyHandler.cs
??? MediatRAPI/
?   ??? Controllers/
?   ?   ??? CustomerController.cs
?   ??? Program.cs
??? README.md
```

## Design Patterns Used

1. **Mediator Pattern**: Via MediatR library to decouple request senders from handlers
2. **Repository Pattern**: For data access abstraction
3. **Dependency Injection**: For loose coupling and testability
4. **CQRS-lite**: Separation of command (Create) and query (Get) operations

## Benefits of This Architecture

- **Separation of Concerns**: Clear boundaries between API, business logic, and data access
- **Testability**: Easy to unit test handlers independently
- **Maintainability**: Changes to handlers don't affect controllers
- **Scalability**: Easy to add new requests and handlers
- **Single Responsibility**: Each handler focuses on one specific operation

## Future Enhancements

- Add FluentValidation for request validation
- Implement persistent storage (SQL Server, PostgreSQL, etc.)
- Add logging with Serilog or NLog
- Implement authentication and authorization
- Add unit tests and integration tests
- Implement CQRS with separate read/write models
- Add domain events
- Implement API versioning

## License

This is a demonstration project for educational purposes.
