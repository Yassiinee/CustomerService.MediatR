using System.Net;
using MediatRAPI.IntegrationTests.Common;

namespace MediatRAPI.IntegrationTests.Infrastructure;

public class HealthCheckTests : IntegrationTestBase
{
    public HealthCheckTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task HealthCheck_ShouldReturnHealthy()
    {
        // Act
        HttpResponseMessage response = await Client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        string content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Healthy");
    }
}