using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Nxio.Core.Database.Models;
using Nxio.Core.Database.Models.Enums;

namespace Nxio.Core.Extensions;

[SuppressMessage("Major Code Smell", "S112:General or reserved exceptions should never be thrown", Justification = "This exception indicates major bug in app logic. App should crash.")]
[SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "This exception indicates major bug in app logic. App should crash.")]
public static class GuildSettingsExtension
{
    public static T GetGuildSettingValue<T>(this GuildSettings setting)
    {
        if (string.IsNullOrWhiteSpace(setting.Value)) throw new Exception("Setting value is empty!");

        switch (setting.Type)
        {
            case GuildSettingType.Int:
                if (int.TryParse(setting.Value, out var intValue))
                    return (T)Convert.ChangeType(intValue, typeof(T), CultureInfo.InvariantCulture);
                throw new Exception("Failed to parse int setting value!");

            case GuildSettingType.Bool:
                if (bool.TryParse(setting.Value, out var boolValue))
                    return (T)Convert.ChangeType(boolValue, typeof(T), CultureInfo.InvariantCulture);
                throw new Exception("Failed to parse bool setting value!");

            case GuildSettingType.Ulong:
                if (ulong.TryParse(setting.Value, out var ulongValue))
                    return (T)Convert.ChangeType(ulongValue, typeof(T), CultureInfo.InvariantCulture);
                throw new Exception("Failed to parse ulong setting value!");

            default:
                throw new Exception("Unknown setting type!");
        }
    }
}