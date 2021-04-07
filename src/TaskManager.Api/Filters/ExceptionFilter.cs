using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManager.Api.Filters
{
    internal class ExceptionFilter : IExceptionFilter
    {
        private static readonly Dictionary<Type, Func<ExceptionContext, IActionResult>> _exceptionMapping = new()
        {
            {typeof(InvalidOperationException), BadRequest},
            {typeof(ArgumentException), BadRequest}
        };

        public void OnException(ExceptionContext context)
        {
            if (!_exceptionMapping.TryGetValue(context.Exception.GetType(), out var responseAction))
            {
                responseAction = Problem;
            }

            context.Result = responseAction.Invoke(context);
        }

        private static IActionResult BadRequest(ExceptionContext context)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Bad request",
                Status = 400,
                Detail = context.Exception.Message,
            };

            return new BadRequestObjectResult(
                problemDetails);
        }

        private IActionResult Problem(ExceptionContext context)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Internal server error",
                Status = 500,
                Detail = context.Exception.Message
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };
        }
    }
}