using Application.Abstractions.Database;

namespace Persistence;

internal sealed class DatabaseHealthChecker : IDatabaseHealthChecker
{
    private readonly IApplicationDbContext _dbContext;

    public DatabaseHealthChecker(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> CanConnectAsync(CancellationToken cancellationToken)
    {
        return _dbContext.CanConnectAsync(cancellationToken);
    }
}
