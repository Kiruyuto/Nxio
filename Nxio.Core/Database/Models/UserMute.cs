namespace Nxio.Core.Database.Models;

public class UserMute : DbEntity<Guid>
{
    public required ulong UserId { get; init; }
    public required ulong GuildId { get; init; }
    public required DateTimeOffset MuteStartUtc { get; init; }
    public required DateTimeOffset MuteEndUtc { get; set; }
    public required string RoleIdsBeforeMute { get; init; }
}