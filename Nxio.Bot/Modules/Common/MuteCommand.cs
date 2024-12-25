using Microsoft.EntityFrameworkCore;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using Nxio.Core.Database;
using Nxio.Core.Database.Models;
using Nxio.Core.Database.Models.Enums;
using Nxio.Core.Extensions;

namespace Nxio.Bot.Modules.Common;

public static class MuteCommand
{
    private static readonly Color RedColor = new(red: 255, green: 0, blue: 0);
    private static readonly Color GreenColor = new(red: 0, green: 255, blue: 0);

    public static async Task<(EmbedProperties embed, bool ephemeral)> Run(ILogger logger, BaseDbContext dbContext, GuildUser targetUser, GatewayClient botUser, NetCord.User commandAuthor, int timeInMinutes, string? reason, Guild guild)
    {
        if (targetUser.Id == commandAuthor.Id) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "You can't shoot yourself", Color = RedColor }, true);
        if (targetUser.IsBot || targetUser.IsSystemUser == true) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "You can't shoot a Bot/SystemUser!" }, true);

        var guildSetting = await dbContext.GuildSettings.FirstOrDefaultAsync(x => x.GuildId == guild.Id && x.Name == GuildSettingName.DefaultMuteRole);
        if (guildSetting == null) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "No default mute role set!" }, true);
        var guildMuteRole = guildSetting.GetGuildSettingValue<ulong>();


        var targetRoles = targetUser.GetRoles(guild).ToList();
        // var triggerRolePos = (await guild.GetUserAsync(commandAuthor.Id)).GetRoles(guild);
        // if (targetRoles.Any(x => x.Position > triggerRolePos)) return "You can't shoot someone with a higher role than you!";
        if (targetRoles.Any(x => (x.Permissions & Permissions.Administrator) != 0)) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "You mute an admin!", Color = RedColor }, true);

        var dbCommandVictim = await dbContext.UserMutes.FirstOrDefaultAsync(x => x.UserId == targetUser.Id);
        if (dbCommandVictim != null) dbCommandVictim.MuteEndUtc = DateTimeOffset.UtcNow.AddMinutes(timeInMinutes);
        else
        {
            dbCommandVictim = new UserMute
            {
                GuildId = guild.Id,
                UserId = targetUser.Id,
                MuteStartUtc = DateTimeOffset.UtcNow,
                MuteEndUtc = DateTimeOffset.UtcNow.AddMinutes(timeInMinutes),
                RoleIdsBeforeMute = string.Join(";", targetRoles.Select(x => x.Id)),
            };

            await dbContext.UserMutes.AddAsync(dbCommandVictim);
        }

        foreach (var role in targetRoles.DistinctBy(role => role.Id))
        {
            logger.LogDebug("Removing: {Role}", role.Name);
            await guild.RemoveUserRoleAsync(targetUser.Id, role.Id);
        }

        await guild.AddUserRoleAsync(targetUser.Id, guildMuteRole);

        await dbContext.SaveChangesAsync();

        var embed = new EmbedProperties
        {
            Author = new EmbedAuthorProperties { Name = commandAuthor.Username, IconUrl = commandAuthor.GetAvatarUrl()?.ToString() },
            Description = $"Muted <@{targetUser.Id}> for {timeInMinutes} minutes!\nReason: {reason}\nMute will end approximately <t:{dbCommandVictim.MuteEndUtc.ToUnixTimeSeconds()}:R>",
            Color = GreenColor
        };

        var dmChannel = await targetUser.GetDMChannelAsync();
        await dmChannel.SendMessageAsync(new MessageProperties { Embeds = [embed] });

        return new ValueTuple<EmbedProperties, bool>(embed, false);
    }
}