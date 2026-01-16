# MediatR API Test Suite

This solution contains comprehensive unit and integration tests for the CustomerService.MediatR API.

## Test Projects

### MediatRAPI.UnitTests
Contains isolated unit tests for:
- **Domain Entities**: Customer class validation and behavior
- **Application Handlers**: Command and query handlers with mocked dependencies
- **Application Behaviors**: Validation pipeline behavior
- **Application Exceptions**: Custom exception classes
- **Validators**: FluentValidation rules testing
- **Configuration**: Swagger configuration options

### MediatRAPI.IntegrationTests
Contains end-to-end integration tests for:
- **Controllers**: API endpoints with real HTTP requests
- **Authentication**: JWT token generation and validation
- **Middleware**: Global exception handling
- **Infrastructure**: Health checks and API versioning
- **Validation**: Complete request/response validation flow

## Running the Tests

### Prerequisites
- .NET 10.0 or later
- Visual Studio 2024 or VS Code with C# extension

### Command Line
```bash
# Run all tests
dotnet test

# Run only unit tests
dotnet test MediatRAPI.UnitTests

# Run only integration tests
dotnet test MediatRAPI.IntegrationTests

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

### Visual Studio
1. Open Test Explorer (Test > Test Explorer)
2. Click "Run All Tests" or select specific tests to run
3. View test results and coverage in the Test Explorer window

## Test Architecture

### Unit Tests
- Use **Moq** for mocking dependencies
- Use **FluentAssertions** for readable assertions
- Follow **AAA pattern** (Arrange, Act, Assert)
- Test one concern per test method
- Use **Theory** tests for data-driven scenarios

### Integration Tests
- Use **WebApplicationFactory** for hosting the API
- Use **In-Memory Repository** for data isolation
- Test complete request/response flows
- Verify authentication and authorization
- Test error handling and edge cases

## Test Data Management

### Unit Tests
- Use mocked dependencies to isolate units under test
- Use builders and test data generators for complex objects
- Keep test data minimal and focused on the test scenario

### Integration Tests
- Use in-memory repository that's cleared between tests
- Generate test tokens for authentication
- Use realistic test data that matches production scenarios

## Code Coverage

The tests aim for high code coverage across:
- ? Domain logic
- ? Application handlers and behaviors  
- ? API controllers and middleware
- ? Validation rules
- ? Exception handling
- ? Authentication and authorization

## Best Practices

1. **Test Naming**: Use descriptive names that explain the scenario
2. **Test Organization**: Group related tests in the same class
3. **Test Independence**: Each test should be isolated and repeatable
4. **Test Performance**: Keep tests fast and focused
5. **Test Maintenance**: Update tests when requirements change

## Example Test Patterns

### Unit Test Example
```csharp
[Fact]
public async Task Handle_ShouldCreateCustomer_AndReturnCustomerId()
{
    // Arrange
    var command = new CreateCustomerCommand("John Doe", "john@example.com");
    _mockRepository.Setup(x => x.AddAsync(It.IsAny<Customer>()));

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Should().NotBeEmpty();
    _mockRepository.Verify(x => x.AddAsync(It.IsAny<Customer>()), Times.Once);
}
```

### Integration Test Example  
```csharp
[Fact]
public async Task CreateCustomer_ShouldReturnOk_WhenValidDataProvided()
{
    // Arrange
    var token = await GetAuthTokenAsync();
    SetAuthorizationHeader(token);
    var command = new CreateCustomerCommand("John Doe", "john@example.com");

    // Act
    var response = await Client.PostAsync("/api/v1.0/customers", CreateJsonContent(command));

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
```