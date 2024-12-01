namespace Nxio.Core.Database.Models;

public class CoinReaction : DbEntity<Guid>
{
    public required Guid UserId { get; set; }
    public virtual User? User { get; set; }
    public required Guid CoinMessageId { get; set; }
    public virtual CoinMessage? CoinMessage { get; set; }
}