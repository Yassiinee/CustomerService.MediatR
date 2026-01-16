using System.Collections.Concurrent;
using MediatRHandlers.Application.Common.Interfaces;
using MediatRHandlers.Domain.Entities;

namespace MediatRAPI.IntegrationTests.Common;

public class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly ConcurrentDictionary<Guid, Customer> _customers = new();

    public Task AddAsync(Customer customer)
    {
        _customers.TryAdd(customer.Id, customer);
        return Task.CompletedTask;
    }

    public Task<Customer> GetByIdAsync(Guid id)
    {
        _customers.TryGetValue(id, out Customer? customer);
        return Task.FromResult(customer!);
    }

    public void Clear()
    {
        _customers.Clear();
    }

    public IEnumerable<Customer> GetAll()
    {
        return _customers.Values;
    }
}