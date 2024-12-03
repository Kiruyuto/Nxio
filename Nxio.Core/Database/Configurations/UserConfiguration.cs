using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nxio.Core.Database.Models;

namespace Nxio.Core.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(static x => x.Id);

        builder.Property(static x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(static x => x.UserDiscordId).IsRequired();
        builder.Property(static x => x.Coins).IsRequired().HasDefaultValue(0);

        builder
            .HasMany(static x => x.CoinReactions)
            .WithOne(static x => x.User)
            .HasForeignKey(static x => x.UserId).OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(static x => x.Upgrades)
            .WithMany(static x => x.Users);
    }
}