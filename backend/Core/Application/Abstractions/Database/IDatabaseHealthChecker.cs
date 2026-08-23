namespace Application.Abstractions.Database;

public interface IDatabaseHealthChecker
{
    Task<bool> CanConnectAsync(CancellationToken cancellationToken);
}
