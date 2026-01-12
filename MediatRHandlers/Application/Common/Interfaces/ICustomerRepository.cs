using MediatRHandlers.Domain.Entities;

namespace MediatRHandlers.Application.Common.Interfaces;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer> GetByIdAsync(Guid id);
}
