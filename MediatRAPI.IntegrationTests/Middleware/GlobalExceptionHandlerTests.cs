using System.Net;
using System.Text.Json;
using MediatRAPI.IntegrationTests.Common;
using MediatRHandlers.Application.Customers.Commands;

namespace MediatRAPI.IntegrationTests.Middleware;

public class GlobalExceptionHandlerTests : IntegrationTestBase
{
    public GlobalExceptionHandlerTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GlobalExceptionHandler_ShouldReturnBadRequest_ForValidationError()
    {
        // Arrange
        string token = await GetAuthTokenAsync();
        SetAuthorizationHeader(token);

        // Send invalid data to trigger validation error
        CreateCustomerCommand invalidCommand = new("", "invalid-email"); // Empty name and invalid email
        StringContent content = CreateJsonContent(invalidCommand);

        // Act
        HttpResponseMessage response = await Client.PostAsync("/api/v1.0/customers", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        string responseContent = await response.Content.ReadAsStringAsync();
        ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.StatusCode.Should().Be(400);
        errorResponse.Message.Should().Be("Validation failed.");
        errorResponse.Details.Should().NotBeEmpty();
        errorResponse.Errors.Should().NotBeNull();
        errorResponse.Errors!.Should().ContainKey("Name");
        errorResponse.Errors.Should().ContainKey("Email");
    }

    [Fact]
    public async Task GlobalExceptionHandler_ShouldReturnNotFound_ForNonExistentCustomer()
    {
        // Arrange
        string token = await GetAuthTokenAsync();
        SetAuthorizationHeader(token);
        Guid nonExistentId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await Client.GetAsync($"/api/v1.0/customers/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        string responseContent = await response.Content.ReadAsStringAsync();
        ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        errorResponse.Should().NotBeNull();
        errorResponse!.StatusCode.Should().Be(404);
        errorResponse.Message.Should().Be("Resource not found.");
        errorResponse.Details.Should().Contain("Customer");
        errorResponse.Details.Should().Contain(nonExistentId.ToString());
    }

    [Fact]
    public async Task GlobalExceptionHandler_ShouldStillWork_WithoutToken_DueToTestConfiguration()
    {
        // Note: In test environment, authorization is permissive, so this won't return Unauthorized
        // Instead, let's test that validation still works without a token

        // Arrange
        CreateCustomerCommand command = new("", "invalid"); // This should trigger validation
        StringContent content = CreateJsonContent(command);

        // Act
        HttpResponseMessage response = await Client.PostAsync("/api/v1.0/customers", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
            "because validation should fail even with permissive auth policy");
    }

    private class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
        public string Timestamp { get; set; } = string.Empty;
    }
}