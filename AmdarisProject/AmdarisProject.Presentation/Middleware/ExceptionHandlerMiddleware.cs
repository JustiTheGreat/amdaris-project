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
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (APNotFoundException e)
            {
                LogError(httpContext, e);
                await CreateResponse(httpContext, $"NotFound: {e.Message}", StatusCodes.Status404NotFound);
            }
            catch (APArgumentException e)
            {
                LogError(httpContext, e);
                await CreateResponse(httpContext, $"BadArgument: {e.Message}", StatusCodes.Status400BadRequest);
            }
            catch (APIllegalStatusException e)
            {
                LogError(httpContext, e);
                await CreateResponse(httpContext, $"IllegalStatus: {e.Message}", StatusCodes.Status400BadRequest);
            }
            catch (APUnauthorizedException e)
            {
                LogError(httpContext, e);
                await CreateResponse(httpContext, $"Unauthorized: {e.Message}", StatusCodes.Status401Unauthorized);
            }
            catch (Exception e)
            {
                LogError(httpContext, e);
                await CreateResponse(httpContext, e.Message);
            }
        }

        private void LogError(HttpContext httpContext, Exception e)
            => _logger.LogError("{Method} {Path}: {Message}", [httpContext.Request.Method, httpContext.Request.Path, e.Message]);

        private async Task CreateResponse(HttpContext httpContext, string message,
            int StatusCode = StatusCodes.Status500InternalServerError)
        {
            string basicErrorMessage = "An unexpected error occured!";
            httpContext.Response.StatusCode =
                _environment.IsDevelopment() ? StatusCode : StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "text/plain";
            string responseMessage = _environment.IsDevelopment() ? message : basicErrorMessage;
            await httpContext.Response.WriteAsync(responseMessage);
        }
    }
}
