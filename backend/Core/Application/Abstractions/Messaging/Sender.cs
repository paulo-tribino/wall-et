using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace Application.Abstractions.Messaging;

internal sealed class Sender : ISender
{
    private readonly IServiceProvider _serviceProvider;

    public Sender(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TResponse>> SendAsync<TResponse>(
        IQuery<TResponse> query,
        CancellationToken cancellationToken)
    {
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResponse));

        dynamic handler = _serviceProvider.GetRequiredService(handlerType);

        return await handler.HandleAsync((dynamic)query, cancellationToken);
    }

    public async Task<Result<TResponse>> SendAsync<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken)
    {
        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(command.GetType(), typeof(TResponse));

        dynamic handler = _serviceProvider.GetRequiredService(handlerType);

        return await handler.HandleAsync((dynamic)command, cancellationToken);
    }

    public async Task<Result> SendAsync(
        ICommand command,
        CancellationToken cancellationToken)
    {
        var handlerType = typeof(ICommandHandler<>)
            .MakeGenericType(command.GetType());

        dynamic handler = _serviceProvider.GetRequiredService(handlerType);

        return await handler.HandleAsync((dynamic)command, cancellationToken);
    }
}
