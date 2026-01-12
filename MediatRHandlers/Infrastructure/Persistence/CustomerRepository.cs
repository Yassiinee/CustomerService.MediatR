using MediatRHandlers.Application.Common.Interfaces;
using MediatRHandlers.Domain.Entities;

namespace MediatRHandlers.Infrastructure.Persistence;

public class CustomerRepository : ICustomerRepository
{
    private static readonly List<Customer> _customers = new();

    public Task AddAsync(Customer customer)
    {
        _customers.Add(customer);
        return Task.CompletedTask;
    }

    public Task<Customer> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_customers.First(c => c.Id == id));
    }
}
