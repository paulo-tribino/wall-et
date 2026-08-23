using SharedKernel;

namespace Application.Abstractions.Messaging;

public interface ISender
{
    Task<Result<TResponse>> SendAsync<TResponse>(
        IQuery<TResponse> query,
        CancellationToken cancellationToken);

    Task<Result<TResponse>> SendAsync<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken);

    Task<Result> SendAsync(
        ICommand command,
        CancellationToken cancellationToken);
}
