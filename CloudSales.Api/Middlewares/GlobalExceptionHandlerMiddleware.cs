using Newtonsoft.Json;

namespace CloudSales.Api.Middlewares;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, ErrorFactory errorFactory)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "An unhandled exception occurred.");

        var errorResponse = errorFactory.GetErrorResponse(exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorResponse.StatusCode;

        var jsonResponse = JsonConvert.SerializeObject(errorResponse);
        return context.Response.WriteAsync(jsonResponse);
    }
}
