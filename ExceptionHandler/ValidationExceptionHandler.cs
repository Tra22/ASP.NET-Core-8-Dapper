using System.Net;
using DapperWebAPIProject.Validation.Result;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace DapperWebAPIProject.ExceptionHandler;
public class ValidationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ValidationExceptionHandler> _logger;

    public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        try
        {
            if (exception is not ValidationException validationException)
            {
                return false;
            }

            _logger.LogError(
                validationException,
                "Exception occurred: {Message}",
                validationException.Message);

            _logger.LogError(validationException, "Validation error occurred.");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var response = new
            {
                message = "Validation failed",
                errors = GroupErrorsByRow(validationException.Errors)
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

            return true;
        }catch (Exception)
        {
            return false;
        }
    }
    private Dictionary<int, Dictionary<string, string[]>> GroupErrorsByRow(IEnumerable<ValidationFailure> errors)
    {
        if(errors is IEnumerable<ValidationFailureList> errorList){
            var errorsByRow = errorList
            .Select((error, index) => new { Error = error, RowNumber = error.index })
            .GroupBy(x => x.RowNumber)
            .ToDictionary(
                g => g.Key,
                g => g
                    .GroupBy(x => x.Error.PropertyName)
                    .ToDictionary(
                        gg => gg.Key,
                        gg => gg.Select(x => x.Error.ErrorMessage).ToArray()
                    )
            );
            return errorsByRow;
        }else {
            var errorsByRow = errors
                .Select((error, index) => new { Error = error, RowNumber = index + 1 }) 
                .GroupBy(x => x.RowNumber)
                .ToDictionary(
                    g => g.Key,
                    g => g
                        .GroupBy(x => x.Error.PropertyName)
                        .ToDictionary(
                            gg => gg.Key,
                            gg => gg.Select(x => x.Error.ErrorMessage).ToArray()
                        )
                );
            return errorsByRow;
        }
    }
}
