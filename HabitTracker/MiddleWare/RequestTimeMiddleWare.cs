using System.Diagnostics;

namespace HabitTracker.MiddleWare
{
    public class RequestTimeMiddleWare : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleWare> _logger;

        public RequestTimeMiddleWare(ILogger<RequestTimeMiddleWare> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();

            await next.Invoke(context);

            stopwatch.Stop();

            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            if (elapsedMilliseconds > 4000)
            {
                var message = 
                    $"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMilliseconds} ms";

                _logger.LogWarning(message);
            }
        }
    }
}