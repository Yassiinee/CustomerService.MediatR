using MediatR;
using MediatRHandlers.Entities;

namespace MediatRHandlers.Requests
{
    public class CreateCustomerRequest : IRequest<int>
    {
        public Customer Customer { get; set; }
    }
}
