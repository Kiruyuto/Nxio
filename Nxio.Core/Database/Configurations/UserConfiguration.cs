using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nxio.Core.Database.Models;

namespace Nxio.Core.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.UserDiscordId).IsRequired();
        builder.Property(x => x.Coins).IsRequired().HasDefaultValue(0);

        builder
            .HasMany(x => x.CoinReactions)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Upgrades)
            .WithMany(x => x.Users);
    }
}