using MediatR;
using MediatRHandlers.Application.Common.Interfaces;
using MediatRHandlers.Domain.Entities;
using Serilog;

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
        Log.Information("Creating customer {CustomerName} with email {Email}", request.Name, request.Email);

        Customer customer = new(request.Name, request.Email);
        await _repository.AddAsync(customer);

        Log.Information("Customer {CustomerId} created successfully", customer.Id);
        return customer.Id;
    }
}
