using MediatR;
using MediatRHandlers.Application.Common.Interfaces;
using MediatRHandlers.Domain.Entities;

namespace MediatRHandlers.Application.Customers.Commands;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken ct)
    {
        Customer customer = new(request.Name, request.Email);
        await _repository.AddAsync(customer);
        return customer.Id;
    }
}
