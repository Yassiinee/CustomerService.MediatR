using MediatR;
using MediatRHandlers.Application.Common.Exceptions;
using MediatRHandlers.Application.Common.Interfaces;
using MediatRHandlers.Application.Customers.Dtos;
using MediatRHandlers.Domain.Entities;

namespace MediatRHandlers.Application.Customers.Queries;

public class GetCustomerByIdQueryHandler(ICustomerRepository repository) : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly ICustomerRepository _repository = repository;

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken ct)
    {
        Customer customer = await _repository.GetByIdAsync(request.Id);

        return customer == null
            ? throw new NotFoundException(nameof(Customer), request.Id)
            : new CustomerDto(customer.Id, customer.Name, customer.Email);
    }
}
