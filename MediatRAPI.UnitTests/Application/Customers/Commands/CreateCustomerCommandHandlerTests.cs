using MediatRHandlers.Application.Common.Interfaces;
using MediatRHandlers.Application.Customers.Commands;
using MediatRHandlers.Domain.Entities;

namespace MediatRAPI.UnitTests.Application.Customers.Commands;

public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _mockRepository;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerCommandHandlerTests()
    {
        _mockRepository = new Mock<ICustomerRepository>();
        _handler = new CreateCustomerCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateCustomer_AndReturnCustomerId()
    {
        // Arrange
        CreateCustomerCommand command = new("John Doe", "john.doe@example.com");
        CancellationToken cancellationToken = CancellationToken.None;

        _mockRepository.Setup(x => x.AddAsync(It.IsAny<Customer>()))
                      .Returns(Task.CompletedTask);

        // Act
        Guid result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeEmpty();
        _mockRepository.Verify(x => x.AddAsync(It.Is<Customer>(c =>
            c.Name == command.Name &&
            c.Email == command.Email)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCorrectCustomerToRepository()
    {
        // Arrange
        CreateCustomerCommand command = new("Jane Smith", "jane.smith@example.com");
        CancellationToken cancellationToken = CancellationToken.None;
        Customer? capturedCustomer = null;

        _mockRepository.Setup(x => x.AddAsync(It.IsAny<Customer>()))
                      .Callback<Customer>(customer => capturedCustomer = customer)
                      .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        capturedCustomer.Should().NotBeNull();
        capturedCustomer!.Name.Should().Be(command.Name);
        capturedCustomer.Email.Should().Be(command.Email);
        capturedCustomer.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("", "test@example.com")]
    [InlineData("Test User", "")]
    [InlineData("   ", "whitespace@example.com")]
    public async Task Handle_ShouldCreateCustomer_WithEmptyOrWhitespaceValues(string name, string email)
    {
        // Arrange
        CreateCustomerCommand command = new(name, email);
        CancellationToken cancellationToken = CancellationToken.None;

        _mockRepository.Setup(x => x.AddAsync(It.IsAny<Customer>()))
                      .Returns(Task.CompletedTask);

        // Act
        Guid result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeEmpty();
        _mockRepository.Verify(x => x.AddAsync(It.Is<Customer>(c =>
            c.Name == command.Name &&
            c.Email == command.Email)), Times.Once);
    }
}