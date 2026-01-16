using System.Net;
using System.Text.Json;
using FluentValidation;
using MediatRHandlers.Application.Common.Exceptions;
using Serilog;

namespace MediatRAPI.Middleware;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        ErrorResponse errorResponse = new()
        {
            StatusCode = (int)statusCode,
            Message = "An internal server error occurred.",
            Details = exception.Message
        };

        switch (exception)
        {
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse
                {
                    StatusCode = (int)statusCode,
                    Message = "Validation failed.",
                    Details = string.Join("; ", validationException.Errors.Select(e => e.ErrorMessage)),
                    Errors = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => string.Join("; ", g.Select(e => e.ErrorMessage))
                        )
                };
                break;

            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorResponse = new ErrorResponse
                {
                    StatusCode = (int)statusCode,
                    Message = "Resource not found.",
                    Details = exception.Message
                };
                break;

            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse
                {
                    StatusCode = (int)statusCode,
                    Message = "Bad request.",
                    Details = exception.Message
                };
                break;

            case ForbiddenAccessException:
                statusCode = HttpStatusCode.Forbidden;
                errorResponse = new ErrorResponse
                {
                    StatusCode = (int)statusCode,
                    Message = "Access forbidden.",
                    Details = exception.Message
                };
                break;

            case ConflictException:
                statusCode = HttpStatusCode.Conflict;
                errorResponse = new ErrorResponse
                {
                    StatusCode = (int)statusCode,
                    Message = "Conflict occurred.",
                    Details = exception.Message
                };
                break;

            case KeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorResponse = new ErrorResponse
                {
                    StatusCode = (int)statusCode,
                    Message = "Resource not found.",
                    Details = exception.Message
                };
                break;

            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                errorResponse = new ErrorResponse
                {
                    StatusCode = (int)statusCode,
                    Message = "Unauthorized access.",
                    Details = exception.Message
                };
                break;

            case ArgumentNullException:
            case ArgumentException:
                statusCode = HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse
                {
                    StatusCode = (int)statusCode,
                    Message = "Bad request.",
                    Details = exception.Message
                };
                break;

            case InvalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse
                {
                    StatusCode = (int)statusCode,
                    Message = "Invalid operation.",
                    Details = exception.Message
                };
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        JsonSerializerOptions options = jsonSerializerOptions;

        string json = JsonSerializer.Serialize(errorResponse, options);
        return context.Response.WriteAsync(json);
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public Dictionary<string, string>? Errors { get; set; }
    public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o");
}
