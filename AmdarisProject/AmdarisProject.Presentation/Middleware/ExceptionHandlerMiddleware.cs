using AmdarisProject.Domain.Exceptions;
using Azure.Core;

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
            catch (APUnauthorizedException e)
            {
                LogError(httpContext, e);
                await CreateResponse(httpContext, "Session expired!", StatusCodes.Status401Unauthorized);
            }
            catch (APConflictException e)
            {
                LogError(httpContext, e);
                await CreateResponse(httpContext, e.Message, StatusCodes.Status409Conflict);
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
            catch (Exception e)
            {
                LogError(httpContext, e);
                await CreateResponse(httpContext, e.Message);
            }
        }

        private void LogError(HttpContext httpContext, Exception e)
            => _logger.LogError("{Method} {Path}: {Message}", [httpContext.Request.Method, httpContext.Request.Path, e.Message]);

        private async Task CreateResponse(HttpContext httpContext, string message,
            int statusCode = StatusCodes.Status500InternalServerError)
        {
            bool showDetails = _environment.IsDevelopment()
                || statusCode == StatusCodes.Status401Unauthorized
                || statusCode == StatusCodes.Status409Conflict;
            string basicErrorMessage = "An unexpected error occured!";
            httpContext.Response.StatusCode = showDetails ? statusCode : StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = ContentType.TextPlain.ToString();
            string responseMessage = showDetails ? message : basicErrorMessage;
            await httpContext.Response.WriteAsync(responseMessage);
        }
    }
}
