using System.Net;
using System.Text.Json;
using Truhome.Business.Exceptions;
using Truhome.Business.Models.Response;

namespace Truhome.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (TruhomeException vex)
        {
            await HandleInternalValidationExceptionAsync(context, vex).ConfigureAwait(false);
        }
        catch (OperationCanceledException oex)
        {
            await HandleOperationCancelledExceptionAsync(context, oex).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex).ConfigureAwait(false);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "{message}", exception.Message);

        var response = new ApiResponse(IsSuccess: false, Error: new
        {
            ErrorCode = HttpStatusCode.InternalServerError,
            ErrorMessage = "Something went wrong, Please try again!"
        });

        return WriteResponse(context, response, HttpStatusCode.InternalServerError);
    }

    private Task HandleOperationCancelledExceptionAsync(HttpContext context, OperationCanceledException exception)
    {
        var response = new ApiResponse(IsSuccess: false, Error: new
        {
            ErrorCode = (HttpStatusCode)499,
            ErrorMessage = "Operation Cancelled"
        });

        return WriteResponse(context, response, (HttpStatusCode)499);
    }

    private Task HandleInternalValidationExceptionAsync(HttpContext context, TruhomeException exception)
    {
        _logger.LogError(exception, "{message}", exception.Message);

        ApiResponse response = new ApiResponse(IsSuccess: false, Error: new
        {
            errorCode = exception.ErrorCode,
            errorMessage = exception.ErrorMessage
        });

        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;

        switch (exception.ErrorCode)
        {
            case "TE401":
                httpStatusCode = HttpStatusCode.Unauthorized;
                break;
            case "TE403":
                httpStatusCode = HttpStatusCode.Forbidden;
                break;
            case "TE404":
                httpStatusCode = HttpStatusCode.NotFound;
                break;
            case "TE410":
                httpStatusCode = HttpStatusCode.Gone;
                break;
            case "TE400":
                httpStatusCode = HttpStatusCode.BadRequest;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = Convert.ToInt32(httpStatusCode);
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static Task WriteResponse(HttpContext context, ApiResponse response, HttpStatusCode statusCode)
    {
        var jsonResponse = JsonSerializer.Serialize(response);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(jsonResponse);
    }
}
