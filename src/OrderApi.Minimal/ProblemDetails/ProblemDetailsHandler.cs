using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.Exceptions;
using OrderApi.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace OrderApi.Minimal.ProblemDetails;

// Globalna obsluga wyjatkow. 
// Przechwytujemy wyjatki i zwracamy ustandaryzowany ProblemDetails
public static class ProblemDetailsHandler
{
    // Metoda rozszerzajaca (Extension Method)
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionHandlerFeature?.Error;

                if (exception is null)
                    return;

                // Match Patterns
                var (statusCode, problemDetails) = exception switch
                {
                    InvalidStateTransitionException ex => (HttpStatusCode.Conflict, new Microsoft.AspNetCore.Mvc.ProblemDetails
                    {
                        Type = "http://localhost:5000/problems/invalid-state-transition",
                        Title = "Invalid state transition",
                        Status = (int)HttpStatusCode.Conflict,
                        Detail = ex.Message,
                        Instance = context.Request.Path
                    }),

                    OrderAlreadyPaidException ex => (HttpStatusCode.Conflict, new Microsoft.AspNetCore.Mvc.ProblemDetails
                    {
                        Type = "http://localhost:5000/problems/order-already-paid",
                        Title = "Order already paid",
                        Status = (int)HttpStatusCode.Conflict,
                        Detail = ex.Message,
                        Instance = context.Request.Path
                    }),

                    OrderNotFoundException ex => (HttpStatusCode.NotFound, new Microsoft.AspNetCore.Mvc.ProblemDetails
                    {
                        Type = "http://localhost:5000/problems/order-not-found",
                        Title = "Order not found",
                        Status = (int)HttpStatusCode.NotFound,
                        Detail = ex.Message,
                        Instance = context.Request.Path
                    }),

                    _ => (HttpStatusCode.InternalServerError, new Microsoft.AspNetCore.Mvc.ProblemDetails
                    {
                        Type = "http://localhost:5000/problems/internal-error",
                        Title = "An unexpected error ocurred",
                        Status = (int)HttpStatusCode.InternalServerError,
                        Detail = exception.Message,
                        Instance = context.Request.Path
                    })
                };

                context.Response.StatusCode = (int) statusCode;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            });
        });
    }
}
