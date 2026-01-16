using MediatRHandlers.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace MediatRAPI.IntegrationTests.Common;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Jwt:Issuer", "TestIssuer");
        builder.UseSetting("Jwt:Audience", "TestAudience");
        builder.UseSetting("Jwt:Key", "ThisIsAVeryLongSecretKeyForTestingPurposesOnly123456789012345678901234567890");

        builder.ConfigureServices(services =>
        {
            // Replace the real repository with an in-memory implementation
            ServiceDescriptor? repositoryDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ICustomerRepository));

            if (repositoryDescriptor != null)
            {
                services.Remove(repositoryDescriptor);
            }

            services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();

            // Replace authorization with a permissive policy for testing
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAssertion(_ => true)
                    .Build();
            });

            // Suppress logging during tests
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Warning);
            });
        });

        builder.UseEnvironment("Testing");
    }
}