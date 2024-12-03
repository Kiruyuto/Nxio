using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nxio.Core.Database.Models;

namespace Nxio.Core.Database.Configurations;

public class CoinReactionConfiguration : IEntityTypeConfiguration<CoinReaction>
{
    public void Configure(EntityTypeBuilder<CoinReaction> builder)
    {
        builder.HasKey(static x => x.Id);

        builder.Property(static x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(static x => x.UserId).IsRequired();
        builder.Property(static x => x.CoinMessageId).IsRequired();
    }
}