using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Presentation.Controllers;
using Azure.Core;

namespace AmdarisProject.Presentation.Middleware
{
    public class ContentTypeMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next,
       IWebHostEnvironment environment)
    {
        private readonly ILogger _logger = logger;
        private readonly RequestDelegate _next = next;
        private readonly IWebHostEnvironment _environment = environment;

        public async Task Invoke(HttpContext httpContext)
        {
            await _next.Invoke(httpContext);
            if (httpContext.Request.Path.Value?.StartsWith($"/{nameof(UserController).Replace("Controller", "")}") ?? true)
                httpContext.Response.ContentType = ContentType.TextPlain.ToString();
            else httpContext.Response.ContentType = ContentType.ApplicationJson.ToString();
        }
    }
}
