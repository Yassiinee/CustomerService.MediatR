using MediatR;
using MediatRHandlers.Repositories;
using MediatRHandlers.Requests;

namespace MediatRHandlers.RequestHandlers
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, int>
    {
        //Inject Validators 
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<int> Handle(CreateCustomerRequest request,
            CancellationToken cancellationToken)
        {
            // First validate the request
            return await _customerRepository.CreateCustomer(request.Customer);
        }
    }
}
