using ExpensesReport.Expenses.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ExpensesReport.Expenses.API.Filters
{
    public class ErrorHandlingFilterAtribute : IExceptionFilter
    {
        public ErrorHandlingFilterAtribute() { }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var code = HttpStatusCode.InternalServerError;
            var type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
            string?[]? errors = Array.Empty<string>();

            if (exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
            }
            else if (exception is BadRequestException badRequestException)
            {
                code = HttpStatusCode.BadRequest;
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                errors = badRequestException.Errors;
            }
            else if (exception is UnauthorizedException)
            {
                code = HttpStatusCode.Unauthorized;
                type = "https://tools.ietf.org/html/rfc7235#section-3.1";
            }

            var problemDetails = new ProblemDetails
            {
                Type = type,
                Title = "An error occurred while processing your request.",
                Status = (int)code,
                Detail = exception.Message,
                Instance = context.HttpContext.Request.Path,
                Extensions = { { "Errors", errors } }

            };

            context.Result = new ObjectResult(problemDetails);
            context.ExceptionHandled = true;
        }
    }
}
