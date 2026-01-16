using System.Net;
using System.Text.Json;
using MediatRAPI.IntegrationTests.Common;
using MediatRHandlers.Application.Customers.Commands;
using MediatRHandlers.Application.Customers.Dtos;
using MediatRHandlers.Domain.Entities;

namespace MediatRAPI.IntegrationTests.Controllers;

public class CustomersControllerTests : IntegrationTestBase
{
    public CustomersControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturnOk_WhenValidDataProvided()
    {
        // Arrange
        string token = await GetAuthTokenAsync();
        SetAuthorizationHeader(token);

        CreateCustomerCommand command = new("John Doe", "john.doe@example.com");
        StringContent content = CreateJsonContent(command);

        // Act
        HttpResponseMessage response = await Client.PostAsync("/api/v1.0/customers", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        string responseContent = await response.Content.ReadAsStringAsync();
        Guid customerId = JsonSerializer.Deserialize<Guid>(responseContent);
        customerId.Should().NotBeEmpty();

        // Verify customer was actually created
        IEnumerable<Customer> customers = Repository.GetAll();
        customers.Should().ContainSingle();
        Customer createdCustomer = customers.First();
        createdCustomer.Name.Should().Be(command.Name);
        createdCustomer.Email.Should().Be(command.Email);
    }

    [Fact]
    public async Task CreateCustomer_ShouldWork_EvenWithoutToken_DueToTestConfiguration()
    {
        // Note: In test environment, we have a permissive authorization policy
        // Arrange
        CreateCustomerCommand command = new("John Doe", "john.doe@example.com");
        StringContent content = CreateJsonContent(command);

        // Act
        HttpResponseMessage response = await Client.PostAsync("/api/v1.0/customers", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        Repository.GetAll().Should().ContainSingle();
    }

    [Theory]
    [InlineData("", "test@example.com")] // Empty name
    [InlineData("Jo", "test@example.com")] // Too short name
    [InlineData("John Doe", "")] // Empty email
    [InlineData("John Doe", "invalid-email")] // Invalid email
    public async Task CreateCustomer_ShouldReturnBadRequest_WhenValidationFails(string name, string email)
    {
        // Arrange
        string token = await GetAuthTokenAsync();
        SetAuthorizationHeader(token);

        CreateCustomerCommand command = new(name, email);
        StringContent content = CreateJsonContent(command);

        // Act
        HttpResponseMessage response = await Client.PostAsync("/api/v1.0/customers", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        Repository.GetAll().Should().BeEmpty();
    }

    [Fact]
    public async Task GetCustomer_ShouldReturnOk_WhenCustomerExists()
    {
        // Arrange
        string token = await GetAuthTokenAsync();
        SetAuthorizationHeader(token);

        // Create a customer first
        CreateCustomerCommand createCommand = new("Jane Smith", "jane.smith@example.com");
        StringContent createContent = CreateJsonContent(createCommand);
        HttpResponseMessage createResponse = await Client.PostAsync("/api/v1.0/customers", createContent);
        Guid customerId = JsonSerializer.Deserialize<Guid>(await createResponse.Content.ReadAsStringAsync());

        // Act
        HttpResponseMessage response = await Client.GetAsync($"/api/v1.0/customers/{customerId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        string responseContent = await response.Content.ReadAsStringAsync();
        CustomerDto? customerDto = JsonSerializer.Deserialize<CustomerDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        customerDto.Should().NotBeNull();
        customerDto!.Id.Should().Be(customerId);
        customerDto.Name.Should().Be(createCommand.Name);
        customerDto.Email.Should().Be(createCommand.Email);
    }

    [Fact]
    public async Task GetCustomer_ShouldReturnNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        string token = await GetAuthTokenAsync();
        SetAuthorizationHeader(token);
        Guid nonExistentId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await Client.GetAsync($"/api/v1.0/customers/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetCustomer_ShouldWork_EvenWithoutToken_DueToTestConfiguration()
    {
        // Note: In test environment, we have a permissive authorization policy
        // but the customer won't exist so we should get NotFound, not Unauthorized

        // Arrange
        Guid customerId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await Client.GetAsync($"/api/v1.0/customers/{customerId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound,
            "because the permissive auth policy allows the request, but customer doesn't exist");
    }
}