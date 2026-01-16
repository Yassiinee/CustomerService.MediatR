using MediatRHandlers.Domain.Entities;

namespace MediatRAPI.UnitTests.Domain.Entities;

public class CustomerTests
{
    [Fact]
    public void Constructor_ShouldCreateCustomer_WithValidNameAndEmail()
    {
        // Arrange
        const string name = "John Doe";
        const string email = "john.doe@example.com";

        // Act
        Customer customer = new(name, email);

        // Assert
        customer.Name.Should().Be(name);
        customer.Email.Should().Be(email);
        customer.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Constructor_ShouldGenerateUniqueIds_ForDifferentCustomers()
    {
        // Arrange
        const string name1 = "John Doe";
        const string email1 = "john.doe@example.com";
        const string name2 = "Jane Smith";
        const string email2 = "jane.smith@example.com";

        // Act
        Customer customer1 = new(name1, email1);
        Customer customer2 = new(name2, email2);

        // Assert
        customer1.Id.Should().NotBe(customer2.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldAcceptEmptyOrWhitespaceName_AsValidInput(string name)
    {
        // Arrange
        const string email = "test@example.com";

        // Act
        Customer customer = new(name, email);

        // Assert
        customer.Name.Should().Be(name);
        customer.Email.Should().Be(email);
        customer.Id.Should().NotBeEmpty();
    }
}