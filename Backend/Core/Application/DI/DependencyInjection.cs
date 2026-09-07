using System.Reflection;
using Application.Abstractions.Messaging;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHandlers();
        services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly, includeInternalTypes: true);
        services.TryAddScoped<ISender>(serviceProvider =>
            new ValidationSenderDecorator(
                new Sender(serviceProvider),
                serviceProvider));

        return services;
    }

    private static IServiceCollection AddHandlers(
        this IServiceCollection services)
    {
        var handlerTypes = ApplicationAssembly.Assembly
            .DefinedTypes
            .Where(IsHandler)
            .SelectMany(type =>
                type.ImplementedInterfaces
                    .Where(IsHandlerInterface)
                    .Select(handlerInterface =>
                        ServiceDescriptor.Scoped(handlerInterface, type.AsType())));

        services.TryAddEnumerable(handlerTypes);

        return services;
    }

    private static bool IsHandler(TypeInfo type)
    {
        return !type.IsAbstract &&
               !type.IsInterface &&
               type.ImplementedInterfaces.Any(IsHandlerInterface);
    }

    private static bool IsHandlerInterface(Type type)
    {
        if (!type.IsGenericType)
            return false;

        var genericType = type.GetGenericTypeDefinition();

        return genericType == typeof(IQueryHandler<,>) ||
               genericType == typeof(ICommandHandler<>) ||
               genericType == typeof(ICommandHandler<,>);
    }
}
