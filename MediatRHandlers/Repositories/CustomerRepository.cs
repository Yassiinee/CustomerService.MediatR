using MediatRHandlers.Entities;

namespace MediatRHandlers.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Dictionary<int, Customer> _customers = new();
        private int _nextId = 1;

        public Task<int> CreateCustomer(Customer customer)
        {
            int customerId = _nextId++;
            _customers[customerId] = customer;
            return Task.FromResult(customerId);
        }

        public Task<Customer?> GetCustomer(int customerId)
        {
            _customers.TryGetValue(customerId, out Customer? customer);
            return Task.FromResult(customer);
        }
    }
}
