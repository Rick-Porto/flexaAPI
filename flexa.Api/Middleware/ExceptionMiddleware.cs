using System.Net;
using System.Text.Json;

namespace flexa.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new ErrorResponse { Error = "An unexpected error occurred" };
        int statusCode = (int)HttpStatusCode.InternalServerError;

        switch (exception)
        {
            case KeyNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                response = new ErrorResponse { Error = exception.Message };
                break;
            case UnauthorizedAccessException:
                statusCode = (int)HttpStatusCode.Forbidden;
                response = new ErrorResponse { Error = exception.Message };
                break;
            default:
                if (_env.IsDevelopment())
                {
                    response = new ErrorResponse { Error = exception.Message, StackTrace = exception.StackTrace };
                }
                break;
        }

        context.Response.StatusCode = statusCode;
        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}

public class ErrorResponse
{
    public string Error { get; set; } = default!;
    public string? StackTrace { get; set; }
}
