using System.Net;
using System.Text.Json;
using MediatRAPI.IntegrationTests.Common;
using MediatRHandlers.Application.Customers.Commands;

namespace MediatRAPI.IntegrationTests.Infrastructure;

public class ApiVersioningTests : IntegrationTestBase
{
    public ApiVersioningTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateCustomer_ShouldWork_WithVersionInUrl()
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
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturnNotFound_WithInvalidVersion()
    {
        // Arrange
        string token = await GetAuthTokenAsync();
        SetAuthorizationHeader(token);

        CreateCustomerCommand command = new("John Doe", "john.doe@example.com");
        StringContent content = CreateJsonContent(command);

        // Act
        HttpResponseMessage response = await Client.PostAsync("/api/v2.0/customers", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("/api/v1/customers")]
    [InlineData("/api/v1.0/customers")]
    public async Task CreateCustomer_ShouldWork_WithDifferentVersionFormats(string endpoint)
    {
        // Arrange
        string token = await GetAuthTokenAsync();
        SetAuthorizationHeader(token);

        CreateCustomerCommand command = new("Test User", "test@example.com");
        StringContent content = CreateJsonContent(command);

        // Act
        HttpResponseMessage response = await Client.PostAsync(endpoint, content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}