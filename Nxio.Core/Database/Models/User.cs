namespace Nxio.Core.Database.Models;

public class User : DbEntity<Guid>
{
    public required ulong UserDiscordId { get; set; }
    public virtual List<CoinReaction> CoinReactions { get; set; } = [];

    // TODO: Add items & Upgrades
}