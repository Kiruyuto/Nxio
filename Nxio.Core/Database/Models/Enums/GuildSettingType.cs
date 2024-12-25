using System.Diagnostics.CodeAnalysis;
// ReSharper disable UnusedMember.Global

namespace Nxio.Core.Database.Models.Enums;

[SuppressMessage("Naming", "CA1720:Identifier contains type name")]
public enum GuildSettingType
{
    Int = 0,
    Bool = 1,
    Ulong=2
}