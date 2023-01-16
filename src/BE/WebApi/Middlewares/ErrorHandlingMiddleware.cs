using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Digitime.Server.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        var problemDetails = new ProblemDetails
        {
            Title = "An error occurred while processing your request.",
            Status = (int)code,
            Detail = exception.Message + " \n " + exception.InnerException?.Message,
            Instance = context.Request.Path
        };

        if (exception is ApplicationException || exception is ArgumentException || exception is InvalidOperationException)
            code = HttpStatusCode.BadRequest;
        else if (exception is UnauthorizedAccessException || exception.Message.Contains("Unauthorized"))
            code = HttpStatusCode.Unauthorized;
        else if (exception is KeyNotFoundException)
            code = HttpStatusCode.NotFound;

        var result = JsonConvert.SerializeObject(problemDetails);
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}
