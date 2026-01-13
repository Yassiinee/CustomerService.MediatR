using Asp.Versioning;
using MediatR;
using MediatRHandlers.Application.Customers.Commands;
using MediatRHandlers.Application.Customers.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediatRAPI.Controllers
{
    [ApiController]
    [Route("api/v{v:apiVersion}/customers")]
    [ApiVersion(1.0)]
    public class CustomersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command)
        {
            Guid id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(new GetCustomerByIdQuery(id)));
        }
    }
}