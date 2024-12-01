namespace Nxio.Core.Database.Models;

public class CoinMessage : DbEntity<Guid>
{
    public required ulong GuildId { get; set; }
    public required ulong MessageId { get; set; }

    public virtual List<CoinReaction> Reactions { get; set; } = [];
}