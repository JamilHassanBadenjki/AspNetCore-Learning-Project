using FirstApi.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstApi.Extensions;

public static class ErrorExtensions
{
    public static IActionResult ToProblem(
        this ControllerBase controller,
        Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation
                => StatusCodes.Status400BadRequest,

            ErrorType.NotFound
                => StatusCodes.Status404NotFound,

            ErrorType.Conflict
                => StatusCodes.Status409Conflict,

            _ => StatusCodes.Status500InternalServerError
        };

        return controller.Problem(
            statusCode: statusCode,
            title: error.Code,
            detail: error.Message);
    }
}