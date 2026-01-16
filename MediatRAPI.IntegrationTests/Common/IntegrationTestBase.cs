using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MediatRAPI.IntegrationTests.Common;

public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient Client;
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly InMemoryCustomerRepository Repository;

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();

        // Get the in-memory repository for test setup/assertions
        Repository = (InMemoryCustomerRepository)factory.Services.GetRequiredService<MediatRHandlers.Application.Common.Interfaces.ICustomerRepository>();

        // Clear any existing data before each test
        Repository.Clear();
    }

    protected async Task<string> GetAuthTokenAsync()
    {
        HttpResponseMessage response = await Client.PostAsync("/api/auth/token", new StringContent("", Encoding.UTF8, "application/json"));

        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Failed to get auth token. Status: {response.StatusCode}, Content: {errorContent}");
        }

        string content = await response.Content.ReadAsStringAsync();
        TokenResponse? tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });

        return tokenResponse!.AccessToken;
    }

    protected void SetAuthorizationHeader(string token)
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    protected static StringContent CreateJsonContent(object obj)
    {
        string json = JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}