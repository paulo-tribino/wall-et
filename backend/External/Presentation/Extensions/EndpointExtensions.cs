using Infrastructure.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Presentation.Endpoints;

namespace Presentation.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var assembly = PresentationAssembly.Assembly;

        var endpointServiceDescriptors = assembly.DefinedTypes
             .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                            type.IsAssignableTo(typeof(IEndpoint)))
             .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type));

        services.TryAddEnumerable(endpointServiceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }
}
