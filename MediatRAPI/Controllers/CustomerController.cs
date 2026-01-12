using MediatR;
using MediatRHandlers.Entities;
using MediatRHandlers.Requests;
using Microsoft.AspNetCore.Mvc;

namespace MediatRAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("customerId")]
        public async Task<Customer?> GetCustomerAsync(int customerId)
        {
            Customer? customerDetails = await _mediator.Send(new GetCustomerRequest() { CustomerId = customerId });

            return customerDetails;
        }

        [HttpPost]
        public async Task<int> CreateCustomerAsync(Customer customer)
        {
            int customerId = await _mediator.Send(new CreateCustomerRequest() { Customer = customer });
            return customerId;
        }
    }
}
