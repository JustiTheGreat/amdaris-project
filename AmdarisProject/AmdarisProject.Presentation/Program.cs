using AmdarisProject.Presentation.Extensions;
using AmdarisProject.Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestCompletionTimeLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
