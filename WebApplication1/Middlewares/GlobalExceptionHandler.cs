using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApplication1.Middlewares
{
    public class GlobalExceptionHandler: IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            //Exception from fluent validation
            if(exception is ValidationException validationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                //Format error message from fluent validation
                var errors = validationException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );

                //Return error message
                await httpContext.Response.WriteAsJsonAsync(
                    new
                    {
                        status = 400,
                        message = "Error validating data",
                        errors = errors
                    }, cancellationToken);
                return true;
            }

            //Serwer errors
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                status = 500,
                message = "Unexpected error occured"
            }, cancellationToken);

            return true;
        }
    }
}
