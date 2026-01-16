# MediatRAPI.UnitTests

Comprehensive unit test suite for the CustomerService.MediatR application, providing fast, isolated tests for all application components.

## 📋 Overview

This test project contains unit tests that verify the behavior of individual components in isolation using mocked dependencies. Tests are organized by layer and feature to maintain clarity and ease of maintenance.

## 🧪 Test Categories

### Domain Tests
- **Customer Entity Tests** (`Domain/Entities/CustomerTests.cs`)
  - Constructor validation
  - Property validation
  - Business rule enforcement
  - Domain invariants

### Application Layer Tests

#### Command Handler Tests
- **CreateCustomerCommandHandler** (`Application/Customers/Commands/CreateCustomerCommandHandlerTests.cs`)
  - Customer creation logic
  - Repository interaction validation
  - Return value verification
  - Error handling scenarios

#### Query Handler Tests
- **GetCustomerByIdQueryHandler** (`Application/Customers/Queries/GetCustomerByIdQueryHandlerTests.cs`)
  - Customer retrieval by ID
  - NotFoundException when customer not found
  - Repository method verification
  - Data mapping validation

#### Validator Tests
- **CreateCustomerCommandValidator** (`Application/Customers/Commands/CreateCustomerCommandValidatorTests.cs`)
  - Name validation rules
  - Email format validation
  - Required field validation
  - Business rule validation

#### Behavior Tests
- **ValidationBehavior** (`Application/Common/Behaviors/ValidationBehaviorTests.cs`)
  - Pipeline validation execution
  - Error aggregation
  - Validation failure handling
  - Request processing flow

#### Exception Tests
- **NotFoundException** (`Application/Common/Exceptions/NotFoundExceptionTests.cs`)
  - Exception message formatting
  - Constructor variations
  - Inheritance validation

## 🔧 Test Infrastructure

### Dependencies
- **xUnit** - Testing framework
- **Moq** - Mocking framework
- **FluentAssertions** - Assertion library
- **Microsoft.Extensions.Logging** - Logging abstractions

### Test Utilities
- **TestData.cs** - Common test data builders and helpers
- **GlobalUsings.cs** - Global using statements for all test files

### Testing Patterns

#### AAA Pattern
All tests follow the **Arrange, Act, Assert** pattern:
```csharp
[Fact]
public async Task Handle_ShouldReturnCustomerDto_WhenCustomerExists()
{
    // Arrange
    var customerId = Guid.NewGuid();
    var customer = new Customer("John Doe", "john@example.com");
    
    // Act
    var result = await _handler.Handle(query, cancellationToken);
    
    // Assert
    result.Should().NotBeNull();
    result.Id.Should().Be(customerId);
}
```

#### Mocking Strategy
- Repository interfaces are mocked using Moq
- External dependencies are isolated
- Test doubles replace infrastructure concerns

#### Reflection Usage
Some tests use reflection to set private properties (like Entity IDs) that cannot be set through public constructors, maintaining domain encapsulation while enabling thorough testing.

## 🚀 Running Unit Tests

### Command Line
```bash
# Run all unit tests
dotnet test MediatRAPI.UnitTests

# Run with detailed output
dotnet test MediatRAPI.UnitTests --logger "console;verbosity=detailed"

# Run with code coverage
dotnet test MediatRAPI.UnitTests --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test MediatRAPI.UnitTests --filter "ClassName=GetCustomerByIdQueryHandlerTests"

# Run tests matching pattern
dotnet test MediatRAPI.UnitTests --filter "Name~Customer"
```

### Visual Studio
1. Open **Test Explorer** (Test → Test Explorer)
2. Build the solution to discover tests
3. Run all tests or select specific test categories
4. View test results and code coverage

## 📊 Test Coverage Goals

| Component | Coverage Target | Status |
|-----------|----------------|--------|
| Domain Entities | 100% | ✅ |
| Command Handlers | 95%+ | ✅ |
| Query Handlers | 95%+ | ✅ |
| Validators | 100% | ✅ |
| Behaviors | 95%+ | ✅ |
| Exceptions | 100% | ✅ |

## 🎯 Testing Best Practices

### Test Naming
- Use descriptive test method names
- Format: `MethodName_Should{ExpectedBehavior}_When{Condition}`
- Examples:
  - `Handle_ShouldReturnCustomerDto_WhenCustomerExists`
  - `Validate_ShouldHaveError_WhenEmailIsInvalid`

### Test Organization
- One test class per production class
- Group related tests within the same test class
- Use nested test classes for complex scenarios

### Test Data
- Use meaningful test data that reflects real-world scenarios
- Keep test data minimal and focused
- Use builders or factory methods for complex objects

### Assertions
- Use FluentAssertions for readable test assertions
- Verify both positive and negative scenarios
- Test boundary conditions and edge cases

## 🔍 Test Structure

```
MediatRAPI.UnitTests/
├── Application/
│   ├── Common/
│   │   ├── Behaviors/
│   │   │   └── ValidationBehaviorTests.cs
│   │   └── Exceptions/
│   │       └── NotFoundExceptionTests.cs
│   └── Customers/
│       ├── Commands/
│       │   ├── CreateCustomerCommandHandlerTests.cs
│       │   └── CreateCustomerCommandValidatorTests.cs
│       └── Queries/
│           └── GetCustomerByIdQueryHandlerTests.cs
├── Domain/
│   └── Entities/
│       └── CustomerTests.cs
├── TestUtilities/
│   └── TestData.cs
└── GlobalUsings.cs
```

## 📝 Adding New Tests

When adding new features, follow this checklist:

1. **Create test class** following naming convention: `{ClassUnderTest}Tests`
2. **Add constructor** to set up mocks and system under test
3. **Write tests** for all public methods and scenarios
4. **Cover edge cases** including error conditions
5. **Verify mocks** are called correctly
6. **Update documentation** if needed

### Example New Test Class
```csharp
public class NewFeatureHandlerTests
{
    private readonly Mock<IDependency> _mockDependency;
    private readonly NewFeatureHandler _handler;

    public NewFeatureHandlerTests()
    {
        _mockDependency = new Mock<IDependency>();
        _handler = new NewFeatureHandler(_mockDependency.Object);
    }

    [Fact]
    public async Task Handle_Should{ExpectedBehavior}_When{Condition}()
    {
        // Arrange
        // Act
        // Assert
    }
}
```

## 🐛 Troubleshooting

### Common Issues
1. **Tests failing due to async/await**: Ensure all async methods are properly awaited
2. **Mock setup issues**: Verify mock setups match the actual method calls
3. **Assertion failures**: Use FluentAssertions for clearer error messages

### Debug Tips
- Use breakpoints in test methods to debug
- Check mock verification failures carefully
- Review test output for detailed error messages
- Use `Times.Once()` or similar to verify exact call counts