using MediatRHandlers.Application.Common.Interfaces;
using MediatRHandlers.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRHandlers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        return services;
    }
}
