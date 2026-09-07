using Application.DI;
using Infrastructure.DI;
using Persistence.DI;
using Presentation.DI;
using Presentation.Extensions;
using Presentation.Middleware;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddPresentation();

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Wall-et API Reference")
            .WithTheme(ScalarTheme.Moon)
            .HideDeveloperTools()
            .SortTagsAlphabetically()
            .SortOperationsByMethod();
    });
}

app.UseCors("CorsPolicy");

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();
