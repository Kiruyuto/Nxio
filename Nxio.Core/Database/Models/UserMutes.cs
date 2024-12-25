namespace Nxio.Core.Database.Models;

public class UserMutes : DbEntity<Guid>
{
    public required ulong UserId { get; set; }
    public required ulong GuildId { get; set; }
    public required DateTimeOffset MuteStart { get; set; }
    public required DateTimeOffset MuteEnd { get; set; }
    public required string RoleIdsBeforeMute { get; set; }
}