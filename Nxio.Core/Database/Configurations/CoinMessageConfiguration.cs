using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nxio.Core.Database.Models;

namespace Nxio.Core.Database.Configurations;

public class CoinMessageConfiguration : IEntityTypeConfiguration<CoinMessage>
{
    public void Configure(EntityTypeBuilder<CoinMessage> builder)
    {
        builder.HasKey(static x => x.Id);

        builder.Property(static x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(static x => x.GuildId).IsRequired();
        builder.Property(static x => x.MessageId).IsRequired();

        builder.HasIndex(static x => new { x.GuildId, x.MessageId }).IsUnique();

        builder
            .HasMany(static x => x.Reactions)
            .WithOne(static x => x.CoinMessage)
            .HasForeignKey(static x => x.CoinMessageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}