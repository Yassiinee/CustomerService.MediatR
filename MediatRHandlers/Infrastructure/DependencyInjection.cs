using FluentValidation;
using MediatR;
using MediatRHandlers.Application.Common.Behaviors;
using MediatRHandlers.Application.Common.Interfaces;
using MediatRHandlers.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRHandlers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // MediatR validation pipeline
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>)
        );

        return services;
    }
}
