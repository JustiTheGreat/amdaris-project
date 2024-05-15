using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Infrastructure.Options;
using AmdarisProject.Infrastructure.Persistance.Extensions;
using AmdarisProject.Presentation;
using AmdarisProject.Presentation.Extensions;
using AmdarisProject.Presentation.Middleware;
using AmdarisProject.Presentation.Options;
using MapsterMapper;
using OnlineBookShop.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

JwtSettings jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
    ?? throw new AmdarisProjectException("Missing JWT settings!");

builder.Services.AddControllers();
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerWithAuthorization()
    .AddInfrastructure()
    .AddApplication()
    .AddScoped<IMapper>(sp => new Mapper(MapsterConfiguration.GetMapsterConfiguration()))
    .Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)))
    .Configure<AssembliesNames>(builder.Configuration.GetSection(nameof(AssembliesNames)))
    .Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)))
    .AddAuthenticationService(jwtSettings);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestCompletionTimeLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
