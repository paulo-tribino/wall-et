using Application.Abstractions.Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.DI;

namespace Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(PersistanceAssembly.Assembly);
    }

    public Task<bool> CanConnectAsync(CancellationToken cancellationToken)
    {
        return Database.CanConnectAsync(cancellationToken);
    }
}
