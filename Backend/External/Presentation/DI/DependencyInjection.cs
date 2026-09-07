using Microsoft.Extensions.DependencyInjection;
using Presentation.Extensions;

namespace Presentation.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddCors(config =>
        {
            config.AddPolicy(name: "CorsPolicy", builder =>
            {
                // Allow any origin for maximum compatibility with mobile apps
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        services.AddEndpoints();

        return services;
    }
}
