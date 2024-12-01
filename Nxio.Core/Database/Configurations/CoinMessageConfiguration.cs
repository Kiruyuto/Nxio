using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nxio.Core.Database.Models;

namespace Nxio.Core.Database.Configurations;

public class CoinMessageConfiguration : IEntityTypeConfiguration<CoinMessage>
{
    public void Configure(EntityTypeBuilder<CoinMessage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.GuildId).IsRequired();
        builder.Property(x => x.MessageId).IsRequired();

        builder.HasIndex(x => new { x.GuildId, x.MessageId }).IsUnique();

        builder
            .HasMany(x => x.Reactions)
            .WithOne(x => x.CoinMessage)
            .HasForeignKey(x => x.CoinMessageId);
    }
}