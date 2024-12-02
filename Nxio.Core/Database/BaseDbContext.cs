using Microsoft.EntityFrameworkCore;
using Nxio.Core.Database.Models;

namespace Nxio.Core.Database;

public class BaseDbContext(DbContextOptions<BaseDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<CoinMessage> CoinMessages { get; set; } = default!;
    public DbSet<CoinReaction> CoinReactions { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Upgrade> Upgrades { get; set; } = default!;
}