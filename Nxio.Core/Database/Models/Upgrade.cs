using System.Diagnostics.CodeAnalysis;
using Nxio.Core.Database.Models.Enums;

namespace Nxio.Core.Database.Models;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class Upgrade : DbEntity<Guid>
{
    public string Name { get; set; } = default!;
    public UpgradeType Type { get; set; }
    public int ValuePerLevel { get; set; }
    public int Price { get; set; }
    public int Level { get; set; }
    public int MaxLevel { get; set; }

    public virtual List<User> Users { get; set; } = [];
}