using Nxio.Core.Database.Models.Enums;

namespace Nxio.Core.Database.Models;

public class GuildSettings : DbEntity<Guid>
{
    public Guid GuildId { get; set; }
    public GuildSettingName Name { get; set; }
    public string Value { get; set; }
    public GuildSettingType Type { get; set; }
}