using MediatR;

namespace MediatRHandlers.Application.Customers.Commands;

public record CreateCustomerCommand(string Name, string Email) : IRequest<Guid>;
