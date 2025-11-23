using HabitTracker.Exceptions;

namespace HabitTracker.MiddleWare;

public class ErrorHandlingMiddleWare : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleWare> _logger;

    public ErrorHandlingMiddleWare(ILogger<ErrorHandlingMiddleWare> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (BadRequestException badRequestException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(badRequestException.Message);
        }
        catch (NotFoundException notFoundException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(notFoundException.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandled exception occurred");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong: " + e.Message);
        }

    }
}