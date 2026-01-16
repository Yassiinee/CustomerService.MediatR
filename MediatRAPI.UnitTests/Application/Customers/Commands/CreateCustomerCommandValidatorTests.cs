using FluentValidation.TestHelper;
using MediatRHandlers.Application.Customers.Commands;

namespace MediatRAPI.UnitTests.Application.Customers.Commands;

public class CreateCustomerCommandValidatorTests
{
    private readonly CreateCustomerCommandValidator _validator;

    public CreateCustomerCommandValidatorTests()
    {
        _validator = new CreateCustomerCommandValidator();
    }

    [Fact]
    public void Should_HaveError_When_Name_IsEmpty()
    {
        // Arrange
        CreateCustomerCommand command = new("", "test@example.com");

        // Act & Assert
        TestValidationResult<CreateCustomerCommand> result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_HaveError_When_Name_IsTooShort()
    {
        // Arrange
        CreateCustomerCommand command = new("Jo", "test@example.com");

        // Act & Assert
        TestValidationResult<CreateCustomerCommand> result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_NotHaveError_When_Name_IsValid()
    {
        // Arrange
        CreateCustomerCommand command = new("John Doe", "test@example.com");

        // Act & Assert
        TestValidationResult<CreateCustomerCommand> result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_HaveError_When_Email_IsEmpty()
    {
        // Arrange
        CreateCustomerCommand command = new("John Doe", "");

        // Act & Assert
        TestValidationResult<CreateCustomerCommand> result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    [InlineData("test.example.com")]
    public void Should_HaveError_When_Email_IsInvalid(string email)
    {
        // Arrange
        CreateCustomerCommand command = new("John Doe", email);

        // Act & Assert
        TestValidationResult<CreateCustomerCommand> result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.co.uk")]
    [InlineData("test123@test123.com")]
    public void Should_NotHaveError_When_Email_IsValid(string email)
    {
        // Arrange
        CreateCustomerCommand command = new("John Doe", email);

        // Act & Assert
        TestValidationResult<CreateCustomerCommand> result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_NotHaveErrors_When_AllFieldsAreValid()
    {
        // Arrange
        CreateCustomerCommand command = new("John Doe", "john.doe@example.com");

        // Act & Assert
        TestValidationResult<CreateCustomerCommand> result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}