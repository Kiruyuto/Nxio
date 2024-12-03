using System.Diagnostics.CodeAnalysis;

namespace Nxio.Core.Database.Models;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class CoinMessage : DbEntity<Guid>
{
    public required ulong GuildId { get; set; }
    public required ulong MessageId { get; set; }

    public virtual List<CoinReaction> Reactions { get; set; } = [];
}