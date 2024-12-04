using System.Diagnostics.CodeAnalysis;

namespace Nxio.Core.Database.Models;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class User : DbEntity<Guid>
{
    /// <summary>
    /// Discord ID of the user (Snowflake)
    /// </summary>
    public required ulong UserDiscordId { get; set; }

    /// <summary>
    /// Coins the user has
    /// </summary>
    public int Coins { get; set; }

    /// <summary>
    /// Time in minutes the user has been muted by others
    /// </summary>
    public int MinutesMutedByOthers { get; set; }

    /// <summary>
    /// Time in minutes the user has muted others
    /// </summary>
    public int MinutesMutedOthers { get; set; }

    /// <summary>
    /// Time in minutes the user has muted themselves trying to hit others
    /// </summary>
    public int MinutesMutedSelf { get; set; }

    /// <summary>
    /// How many times user tried to hit others (/Roulette)
    /// </summary>
    public int HitAttempts { get; set; }

    /// <summary>
    /// How many times user successfully hit others (/Roulette)
    /// </summary>
    public int SuccessfulHits { get; set; }

    public virtual List<CoinReaction> CoinReactions { get; set; } = [];

    public virtual List<Upgrade> Upgrades { get; set; } = [];
}