using System.Net;
using System.Text.Json;

namespace FITLIFENETWORKPROYECTO.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado en la API");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status409Conflict,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            statusCode,
            error = GetErrorName(statusCode),
            message = exception.Message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static string GetErrorName(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "BadRequest",
        StatusCodes.Status404NotFound => "NotFound",
        StatusCodes.Status409Conflict => "Conflict",
        _ => "InternalServerError"
    };
}
