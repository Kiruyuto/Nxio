using System.Net;
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
        if (targetUser.Id == commandAuthor.Id) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "You can't mute yourself", Color = RedColor }, true);
        if (targetUser.IsBot || targetUser.IsSystemUser == true) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "You can't mute `bots` and `system users`!" }, true);

        var guildSetting = await dbContext.GuildSettings.FirstOrDefaultAsync(x => x.GuildId == guild.Id && x.Name == GuildSettingName.DefaultMuteRole);
        if (guildSetting == null) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "No default mute role set!" }, true);
        var guildMuteRole = guildSetting.GetGuildSettingValue<ulong>();

        var botRoles = (await guild.GetUserAsync(botUser.Id)).GetRoles(guild).ToList();
        var maxBot = botRoles.MaxBy(x => x.Position)!.Position;
        var targetRoles = targetUser.GetRoles(guild).Where(x => x.Tags is { GuildConnections: false, IsPremiumSubscriber: false, IsAvailableForPurchase: false }).ToList();
        var maxTarget = targetRoles.MaxBy(x => x.Position)!.Position;
        var commandAuthorRoles = (await guild.GetUserAsync(commandAuthor.Id)).GetRoles(guild).ToList();
        var maxAuthor = commandAuthorRoles.MaxBy(x => x.Position)!.Position;

        logger.LogDebug("MaxTarget: {MaxTarget}, MaxAuthor: {MaxAuthor}, MaxBot: {MaxBot}", maxTarget, maxAuthor, maxBot);

        if (maxTarget > maxAuthor) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "You can't mute with someone of higher role **than you**!", Color = RedColor }, true);
        if (maxTarget > maxBot) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "You can't mute someone with higher role **than me**!", Color = RedColor }, true);

        var dbCommandTarget = await dbContext.UserMutes.FirstOrDefaultAsync(x => x.UserId == targetUser.Id);
        if (dbCommandTarget != null) dbCommandTarget.MuteEndUtc = DateTimeOffset.UtcNow.AddMinutes(timeInMinutes);
        else
        {
            dbCommandTarget = new UserMute
            {
                GuildId = guild.Id,
                UserId = targetUser.Id,
                MuteStartUtc = DateTimeOffset.UtcNow,
                MuteEndUtc = DateTimeOffset.UtcNow.AddMinutes(timeInMinutes),
                RoleIdsBeforeMute = string.Join(";", targetRoles.Select(x => x.Id)),
            };

            await dbContext.UserMutes.AddAsync(dbCommandTarget);
        }

        foreach (var role in targetRoles.DistinctBy(role => role.Id))
        {
            logger.LogDebug("Removing: {RoleName} ({RoleId}) from {Username} ({UserId})", role.Name, role.Id, targetUser.Username, targetUser.Id);
            await guild.RemoveUserRoleAsync(targetUser.Id, role.Id);
        }

        await guild.AddUserRoleAsync(targetUser.Id, guildMuteRole);

        await dbContext.SaveChangesAsync();

        var embed = new EmbedProperties
        {
            Author = new EmbedAuthorProperties { Name = commandAuthor.Username, IconUrl = commandAuthor.GetAvatarUrl()?.ToString() },
            Description = $"Muted <@{targetUser.Id}> for {timeInMinutes} minutes!\nReason: {reason}\nMute will end approximately <t:{dbCommandTarget.MuteEndUtc.ToUnixTimeSeconds()}:R>",
            Color = GreenColor
        };

        try
        {
            var dmChannel = await targetUser.GetDMChannelAsync();
            await dmChannel.SendMessageAsync(new MessageProperties { Embeds = [embed] });
        }
        catch (RestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Forbidden)
                logger.LogDebug(ex, "Failed to send DM to {Username} ({UserId}). User has disabled DMs or blocked the client!", targetUser.Username, targetUser.Id);
            else throw;
        }

        return new ValueTuple<EmbedProperties, bool>(embed, false);
    }
}