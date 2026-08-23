using Application.Abstractions.Authentication;
using Application.Abstractions.Hashers;
using Infrastructure.Authentication;
using Infrastructure.Configurations;
using Infrastructure.Hashers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configs
        services.AddConfigs(configuration);

        services.AddHttpClient();

        // Auth
        services.AddAuth();

        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddTransient<IPasswordHasher, PasswordHasher>();

        return services;
    }

    private static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfigurations>(configuration.GetSection(nameof(JwtConfigurations)));

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        // Auth
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddAuthorization();

        // Auth Handlers
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}
