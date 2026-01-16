using System.Reflection;
using MediatRHandlers.Application.Common.Exceptions;
using MediatRHandlers.Application.Common.Interfaces;
using MediatRHandlers.Application.Customers.Dtos;
using MediatRHandlers.Application.Customers.Queries;
using MediatRHandlers.Domain.Entities;

namespace MediatRAPI.UnitTests.Application.Customers.Queries;

public class GetCustomerByIdQueryHandlerTests
{
    private readonly Mock<ICustomerRepository> _mockRepository;
    private readonly GetCustomerByIdQueryHandler _handler;

    public GetCustomerByIdQueryHandlerTests()
    {
        _mockRepository = new Mock<ICustomerRepository>();
        _handler = new GetCustomerByIdQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCustomerDto_WhenCustomerExists()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Customer customer = new("John Doe", "john.doe@example.com");
        GetCustomerByIdQuery query = new(customerId);
        CancellationToken cancellationToken = CancellationToken.None;

        // Use reflection to set the Id since Customer constructor doesn't allow it
        PropertyInfo? idProperty = typeof(Customer).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        idProperty!.SetValue(customer, customerId);

        _mockRepository.Setup(x => x.GetByIdAsync(customerId))
                      .ReturnsAsync(customer);

        // Act
        CustomerDto result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(customerId);
        result.Name.Should().Be(customer.Name);
        result.Email.Should().Be(customer.Email);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCustomerDoesNotExist()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        GetCustomerByIdQuery query = new(customerId);
        CancellationToken cancellationToken = CancellationToken.None;

        _mockRepository.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync((Customer?)null);

        // Act & Assert
        NotFoundException exception = await Assert.ThrowsAsync<MediatRHandlers.Application.Common.Exceptions.NotFoundException>(
            () => _handler.Handle(query, cancellationToken));

        exception.Message.Should().Contain("Customer");
        exception.Message.Should().Contain(customerId.ToString());
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Customer customer = new("Test User", "test@example.com");
        GetCustomerByIdQuery query = new(customerId);
        CancellationToken cancellationToken = CancellationToken.None;

        // Use reflection to set the Id
        PropertyInfo? idProperty = typeof(Customer).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        idProperty!.SetValue(customer, customerId);

        _mockRepository.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync(customer);

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _mockRepository.Verify(x => x.GetByIdAsync(customerId), Times.Once);
    }
}