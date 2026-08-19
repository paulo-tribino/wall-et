using FluentValidation;
using Infrastructure.DI;
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

        services.AddValidatorsFromAssembly(PresentationAssembly.Assembly, includeInternalTypes: true);

        services.AddEndpoints();

        return services;
    }
}
