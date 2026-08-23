namespace Application.Abstractions.Database;

public interface IApplicationDbContext
{
    Task<bool> CanConnectAsync(CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
