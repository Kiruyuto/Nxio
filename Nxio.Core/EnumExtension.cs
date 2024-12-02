using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Nxio.Core;

public static class EnumExtensions
{
    public static string GetEnumMemberName(this Enum enumValue)
        => enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.Name
           ?? enumValue.ToString();
}