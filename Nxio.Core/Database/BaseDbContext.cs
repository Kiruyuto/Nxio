using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Nxio.Core.Database.Models;

namespace Nxio.Core.Database;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
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
    public DbSet<GuildSettings> GuildSettings { get; set; } = default!;
    public DbSet<UserMutes> UserMutes { get; set; } = default!;
}