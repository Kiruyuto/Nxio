using Nxio.Core.Database.Models.Enums;

namespace Nxio.Core.Database.Models;

public class GuildSettings : DbEntity<Guid>
{
    public required ulong GuildId { get; set; }
    public required GuildSettingName Name { get; set; }
    public required string Value { get; set; }
    public required GuildSettingType Type { get; set; }
}