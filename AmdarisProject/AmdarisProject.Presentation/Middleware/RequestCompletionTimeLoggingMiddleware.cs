namespace AmdarisProject.Presentation.Middleware
{
    public class RequestCompletionTimeLoggingMiddleware(ILogger<RequestCompletionTimeLoggingMiddleware> logger, RequestDelegate next)
    {
        private readonly ILogger _logger = logger;
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            DateTimeOffset start = DateTimeOffset.UtcNow;
            await _next.Invoke(httpContext);
            _logger.LogError("{Method} {Path}: {Duration}",
                [httpContext.Request.Method, httpContext.Request.Path, DateTimeOffset.UtcNow - start]);
        }
    }
}
