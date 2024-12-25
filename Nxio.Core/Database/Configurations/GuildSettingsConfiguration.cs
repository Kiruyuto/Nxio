using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nxio.Core.Database.Models;

namespace Nxio.Core.Database.Configurations;

public class GuildSettingsConfiguration : IEntityTypeConfiguration<GuildSettings>
{
    public void Configure(EntityTypeBuilder<GuildSettings> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.GuildId).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Value).IsRequired().HasMaxLength(512); // TODO: Subject to change
        builder.Property(x => x.Type).IsRequired();
    }
}