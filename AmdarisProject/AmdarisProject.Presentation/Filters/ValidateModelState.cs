using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace AmdarisProject.Presentation.Filters
{
    public class ValidateModelState : ActionFilterAttribute
    {
        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            StringBuilder stringBuilder = new();

            context.ModelState.AsEnumerable()
                .Aggregate(new List<ModelError>(), (result, entry) => [.. result, .. entry.Value?.Errors])
                .Where(modelError => modelError is not null)
                .ToList()
                .ForEach(modelError => stringBuilder.AppendLine($"Model state error: {modelError.ErrorMessage}"));

            if (stringBuilder.Length > 0)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync(stringBuilder.ToString());
            }
        }
    }
}
