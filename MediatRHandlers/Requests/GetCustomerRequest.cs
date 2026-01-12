using MediatR;
using MediatRHandlers.Entities;

namespace MediatRHandlers.Requests
{
    public class GetCustomerRequest : IRequest<Customer?>
    {
        public int CustomerId { get; set; }
    }
}
