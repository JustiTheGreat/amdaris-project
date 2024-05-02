using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace AmdarisProject.Presentation.Filters
{
    public class ValidateGuid(params string[] keys) : ActionFilterAttribute
    {
        private readonly List<string> _keys = keys.ToList();

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            StringBuilder stringBuilder = new();

            _keys.ForEach(key =>
            {
                if (!context.ActionArguments.TryGetValue(key, out var value))
                    return;

                if (!Guid.TryParse(value?.ToString(), out var guid))
                    stringBuilder.AppendLine($"Invalid Guid: {key} => {value}");
            });

            if (stringBuilder.Length > 0)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync(stringBuilder.ToString());
            }
        }
    }
}
