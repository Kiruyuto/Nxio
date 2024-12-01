using Microsoft.EntityFrameworkCore;

namespace Nxio.Core.Database;

public class BaseDbContext(DbContextOptions<BaseDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}