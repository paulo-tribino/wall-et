using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Database;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }

    Task<bool> CanConnectAsync(CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
