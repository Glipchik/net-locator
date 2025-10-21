using System.Net;
using System.Text.Json;
using NetLocator.BatchProcessingService.Shared.Exceptions;

namespace NetLocator.BatchProcessingService.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (InvalidRequestException ex)
        {
            logger.LogError(ex, "Invalid request exception has been raised");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new { error = ex.Message });
            await context.Response.WriteAsync(result);
        }
        catch (BatchNotFoundException ex)
        {
            logger.LogError(ex, "Requested batch was not found");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new { error = "Requested batch was not found" });
            await context.Response.WriteAsync(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new { error = "Something went wrong. Try again later" });
            await context.Response.WriteAsync(result);
        }
    }
}
