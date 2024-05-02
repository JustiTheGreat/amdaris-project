using AmdarisProject.Domain.Exceptions;

namespace AmdarisProject.Presentation.Middleware
{
    public class ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next,
        IWebHostEnvironment environment)
    {
        private readonly ILogger _logger = logger;
        private readonly RequestDelegate _next = next;
        private readonly IWebHostEnvironment _environment = environment;

        public async Task Invoke(HttpContext httpContext)
        {
            string basicErrorMessage = "An unexpected error occured!";

            try
            {
                await _next.Invoke(httpContext);
            }
            catch (APNotFoundException e)
            {
                _logger.LogError("{Method} {Path}: {Message}", [httpContext.Request.Method, httpContext.Request.Path, e.Message]);
                httpContext.Response.StatusCode =
                    _environment.IsDevelopment() ? StatusCodes.Status404NotFound : StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "text/plain";
                string responseMessage = _environment.IsDevelopment() ? $"Not found: {e.Message}!" : basicErrorMessage;
                await httpContext.Response.WriteAsync(responseMessage);
            }
            catch (APArgumentException e)
            {
                _logger.LogError("{Method} {Path}: {Message}", [httpContext.Request.Method, httpContext.Request.Path, e.Message]);
                httpContext.Response.StatusCode =
                    _environment.IsDevelopment() ? StatusCodes.Status400BadRequest : StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "text/plain";
                string responseMessage = _environment.IsDevelopment() ? $"BadArgument: {e.Message}!" : basicErrorMessage;
                await httpContext.Response.WriteAsync(responseMessage);
            }
            catch (APIllegalStatusException e)
            {
                _logger.LogError("{Method} {Path}: {Message}", [httpContext.Request.Method, httpContext.Request.Path, e.Message]);
                httpContext.Response.StatusCode =
                    _environment.IsDevelopment() ? StatusCodes.Status400BadRequest : StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "text/plain";
                string responseMessage = _environment.IsDevelopment() ? $"IllegalStatus: {e.Message}!" : basicErrorMessage;
                await httpContext.Response.WriteAsync(responseMessage);
            }
            catch (Exception e)
            {
                _logger.LogError("{Method} {Path}: {Message}", [httpContext.Request.Method, httpContext.Request.Path, e.Message]);
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "text/plain";
                string responseMessage = _environment.IsDevelopment() ? e.Message : basicErrorMessage;
                await httpContext.Response.WriteAsync(responseMessage);
            }
        }
    }
}
