using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nxio.Core.Database.Models;

namespace Nxio.Core.Database.Configurations;

public class UserMutesConfiguration : IEntityTypeConfiguration<UserMutes>
{
    public void Configure(EntityTypeBuilder<UserMutes> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.GuildId).IsRequired();
        builder.Property(x => x.MuteStartUtc).IsRequired();
        builder.Property(x => x.MuteEndUtc).IsRequired();
        builder.Property(x => x.RoleIdsBeforeMute).IsRequired().HasMaxLength(6_000); // Server cannot have more than 250 (thus 250*SnowflakeId) roles + Separators + Some extra error margin
    }
}