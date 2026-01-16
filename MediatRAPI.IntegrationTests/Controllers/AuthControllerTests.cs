using System.Net;
using System.Text.Json;
using MediatRAPI.IntegrationTests.Common;

namespace MediatRAPI.IntegrationTests.Controllers;

public class AuthControllerTests : IntegrationTestBase
{
    public AuthControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetToken_ShouldReturnOk_WithValidToken()
    {
        // Act
        HttpResponseMessage response = await Client.PostAsync("/api/auth/token", new StringContent(""));

        // Debug what we're actually getting
        string responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK,
            $"because the auth endpoint should work. Response content: {responseContent}");

        responseContent.Should().NotBeNullOrEmpty();

        // Try to deserialize to verify it's valid JSON
        TokenResponse? tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });

        tokenResponse.Should().NotBeNull();
        tokenResponse!.AccessToken.Should().NotBeNullOrEmpty();

        // Verify the token is a valid JWT format (has 3 parts separated by dots)
        string[] tokenParts = tokenResponse.AccessToken.Split('.');
        tokenParts.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetToken_ShouldReturnDifferentTokens_OnMultipleCalls()
    {
        // Act
        HttpResponseMessage response1 = await Client.PostAsync("/api/auth/token", new StringContent(""));
        HttpResponseMessage response2 = await Client.PostAsync("/api/auth/token", new StringContent(""));

        // Assert
        response1.StatusCode.Should().Be(HttpStatusCode.OK);
        response2.StatusCode.Should().Be(HttpStatusCode.OK);

        string content1 = await response1.Content.ReadAsStringAsync();
        string content2 = await response2.Content.ReadAsStringAsync();

        TokenResponse? token1 = JsonSerializer.Deserialize<TokenResponse>(content1, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });
        TokenResponse? token2 = JsonSerializer.Deserialize<TokenResponse>(content2, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });

        token1!.AccessToken.Should().NotBe(token2!.AccessToken);
    }

    private class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}