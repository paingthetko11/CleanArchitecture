using CleanArchitecture.Api.Contracts;
using FluentValidation;
using System.Text.Json;

namespace CleanArchitecture.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (ValidationException exception)
        {
            var errors = exception.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }).ToArray();
            await WriteAsync(context, StatusCodes.Status400BadRequest, "Validation Failed", errors);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled API exception");
            await WriteAsync(context, StatusCodes.Status500InternalServerError, "Internal Server Error", (object?)null);
        }
    }

    private static async Task WriteAsync<T>(HttpContext context, int statusCode, string message, T? data)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(ApiResponse<T>.Fail(statusCode, message, data), JsonSerializerOptions.Web));
    }
}
