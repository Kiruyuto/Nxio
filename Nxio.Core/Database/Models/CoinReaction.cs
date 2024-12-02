namespace Nxio.Core.Database.Models;

public class CoinReaction : DbEntity<Guid>
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public Guid CoinMessageId { get; set; }
    public virtual CoinMessage CoinMessage { get; set; } = default!;
}