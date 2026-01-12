using MediatR;
using MediatRHandlers.Application.Common.Interfaces;
using MediatRHandlers.Application.Customers.Dtos;
using MediatRHandlers.Domain.Entities;

namespace MediatRHandlers.Application.Customers.Queries;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly ICustomerRepository _repository;

    public GetCustomerByIdQueryHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken ct)
    {
        Customer customer = await _repository.GetByIdAsync(request.Id);
        return new CustomerDto(customer.Id, customer.Name, customer.Email);
    }
}
