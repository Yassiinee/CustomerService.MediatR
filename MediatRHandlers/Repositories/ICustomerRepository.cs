using MediatRHandlers.Entities;

namespace MediatRHandlers.Repositories
{
    public interface ICustomerRepository
    {
        Task<int> CreateCustomer(Customer customer);
        Task<Customer?> GetCustomer(int customerId);
    }
}
