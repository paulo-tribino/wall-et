using Application.Abstractions.Database;
using Microsoft.EntityFrameworkCore;
using Persistence.DI;

namespace Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(PersistanceAssembly.Assembly);
    }
}
