using Nxio.Core.Database.Models.Enums;

namespace Nxio.Core.Database.Models;

public class GuildSettings : DbEntity<Guid>
{
    public required ulong GuildId { get; init; }
    public required GuildSettingName Name { get; init; }
    public required string Value { get; init; }
    public required GuildSettingType Type { get; init; }
}