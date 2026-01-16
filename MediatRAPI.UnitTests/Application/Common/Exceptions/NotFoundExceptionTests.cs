using MediatRHandlers.Application.Common.Exceptions;

namespace MediatRAPI.UnitTests.Application.Common.Exceptions;

public class NotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithNameAndKey_ShouldSetCorrectMessage()
    {
        // Arrange
        const string entityName = "Customer";
        Guid key = Guid.NewGuid();

        // Act
        NotFoundException exception = new(entityName, key);

        // Assert
        exception.Message.Should().Be($"Entity \"{entityName}\" ({key}) was not found.");
    }

    [Fact]
    public void Constructor_WithMessage_ShouldSetCorrectMessage()
    {
        // Arrange
        const string customMessage = "Custom not found message";

        // Act
        NotFoundException exception = new(customMessage);

        // Assert
        exception.Message.Should().Be(customMessage);
    }

    [Theory]
    [InlineData("User", 123)]
    [InlineData("Product", "ABC123")]
    [InlineData("Order", 999)]
    public void Constructor_WithDifferentKeyTypes_ShouldFormatCorrectly(string entityName, object key)
    {
        // Act
        NotFoundException exception = new(entityName, key);

        // Assert
        exception.Message.Should().Be($"Entity \"{entityName}\" ({key}) was not found.");
    }
}