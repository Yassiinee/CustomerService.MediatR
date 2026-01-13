using MediatR;
using Serilog;

namespace MediatRHandlers.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Log.Information("Handling {RequestName} {@Request}", typeof(TRequest).Name, request);

            TResponse response = await next(cancellationToken);

            Log.Information("Handled {RequestName} {@Response}", typeof(TRequest).Name, response);

            return response;
        }
    }
}
