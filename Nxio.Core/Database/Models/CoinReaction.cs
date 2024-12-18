﻿using System.Diagnostics.CodeAnalysis;

namespace Nxio.Core.Database.Models;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class CoinReaction : DbEntity<Guid>
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;
    public Guid CoinMessageId { get; set; }
    public virtual CoinMessage CoinMessage { get; set; } = default!;
}