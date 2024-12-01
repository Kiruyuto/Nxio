using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nxio.Core.Database.Models;

namespace Nxio.Core.Database.Configurations;

public class CoinReactionConfiguration : IEntityTypeConfiguration<CoinReaction>
{
    public void Configure(EntityTypeBuilder<CoinReaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.CoinMessageId).IsRequired();
    }
}