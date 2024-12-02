namespace Nxio.Core.Database.Models;

public class User : DbEntity<Guid>
{
    public required ulong UserDiscordId { get; set; }
    public required int Coins { get; set; }
    public virtual List<CoinReaction> CoinReactions { get; set; } = [];

    public virtual List<Upgrade> Upgrades { get; set; } = [];
}