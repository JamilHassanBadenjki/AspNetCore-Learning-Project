using Microsoft.AspNetCore.Diagnostics;

namespace FirstApi.ErrorHandling;


public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Unhandled exception while processing {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path);

        await Results.Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            title: "An unexpected error occurred.",
            detail: "An unexpected error occurred while processing the request.")
            .ExecuteAsync(httpContext);

        return true;
    }

private static string GetTitle(int statusCode)
{
    return statusCode switch
    {
        StatusCodes.Status404NotFound
            => "Resource not found",

        _ => "An unexpected error occurred"
    };
}
}