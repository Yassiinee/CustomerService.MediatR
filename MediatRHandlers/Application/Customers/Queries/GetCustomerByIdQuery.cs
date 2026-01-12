using MediatR;
using MediatRHandlers.Application.Customers.Dtos;

namespace MediatRHandlers.Application.Customers.Queries;

public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto>;
