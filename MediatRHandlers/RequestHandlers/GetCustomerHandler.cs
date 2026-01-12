using MediatR;
using MediatRHandlers.Entities;
using MediatRHandlers.Repositories;
using MediatRHandlers.Requests;

namespace MediatRHandlers.RequestHandlers
{
    public class GetCustomerHandler : IRequestHandler<GetCustomerRequest, Customer?>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer?> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetCustomer(request.CustomerId);
        }
    }
}
