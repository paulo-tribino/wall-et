using Application.Abstractions.Database;

namespace Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IApplicationDbContext _dbContext;

    public UnitOfWork(IApplicationDbContext context)
    {
        _dbContext = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
