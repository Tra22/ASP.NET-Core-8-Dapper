using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace DapperWebAPIProject.ExceptionHandler;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unexpected error occurred.");
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";
        var response = new
        {
            message = "An unexpected error occurred.",
            detail = "Please contact support."
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        return true;
    }
}