using Application.Abstractions.Database;
using Application.Abstractions.Respositories;
using Application.Abstractions.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Persistence.Views;

namespace Persistence.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services
            .AddHealthCheck()
            .AddRepositories()
            .AddViews();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    private static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddScoped<IUserView, UserView>();

        return services;
    }

    private static IServiceCollection AddHealthCheck(this IServiceCollection services)
    {
        services.AddScoped<IDatabaseHealthChecker, DatabaseHealthChecker>();

        return services;
    }
}
