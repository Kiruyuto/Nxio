using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nxio.Core.Database.Models;

namespace Nxio.Core.Database.Configurations;

public class UpgradeConfiguration : IEntityTypeConfiguration<Upgrade>
{
    public void Configure(EntityTypeBuilder<Upgrade> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.ValuePerLevel).HasDefaultValue(0).IsRequired();
        builder.Property(x => x.Price).HasDefaultValue(int.MaxValue).IsRequired(); // High price to prevent buying if admin forgets to set it
        builder.Property(x => x.Level).HasDefaultValue(1).IsRequired();
        builder.Property(x => x.MaxLevel).HasDefaultValue(1).IsRequired();
    }
}