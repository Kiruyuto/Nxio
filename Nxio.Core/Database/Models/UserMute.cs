namespace Nxio.Core.Database.Models;

public class UserMute : DbEntity<Guid>
{
    public required ulong UserId { get; set; }
    public required ulong GuildId { get; set; }
    public required DateTimeOffset MuteStartUtc { get; set; }
    public required DateTimeOffset MuteEndUtc { get; set; }
    public required string RoleIdsBeforeMute { get; set; }
}