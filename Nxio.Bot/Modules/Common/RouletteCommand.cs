using Microsoft.EntityFrameworkCore;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using Nxio.Core;
using Nxio.Core.Database;
using User = Nxio.Core.Database.Models.User;

namespace Nxio.Bot.Modules.Common;

public static class RouletteCommand
{
    public const int MinValue = 1;
    public const int MaxValue = 15;
    private static readonly Color RedColor = new(red: 255, green: 0, blue: 0);
    private static readonly Color GreenColor = new(red: 0, green: 255, blue: 0);

    public static async Task<(EmbedProperties embed, bool ephemeral)> Run(ILogger logger, BaseDbContext dbContext, GuildUser targetUser, GatewayClient botUser, NetCord.User commandAuthor, int timeInMinutes, Guild guild)
    {
        if (timeInMinutes is < MinValue or > MaxValue) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = $"Duration must be between {MinValue} and {MaxValue} minutes!", Color = RedColor }, true);
        if (targetUser.Id == commandAuthor.Id) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "You can't shoot yourself", Color = RedColor }, true);
        if (targetUser.IsBot) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "You can't shoot a bot!" }, true);

        var botRoles = (await guild.GetUserAsync(botUser.Id)).GetRoles(guild).ToList();
        var triggerRolePos = (await guild.GetUserAsync(commandAuthor.Id)).GetRoles(guild);
        if (!botRoles.Any(x => x.Position >= triggerRolePos.Max(z => z.Position)))
            return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "You can't play this game because you have higher permissions than me!", Color = RedColor }, true);

        var targetRoles = targetUser.GetRoles(guild).ToList();
        // if (targetRoles.Any(x => x.Position > triggerRolePos)) return "You can't shoot someone with a higher role than you!";
        if (targetRoles.Any(x => (x.Permissions & Permissions.Administrator) != 0)) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "You can't shoot an admin!", Color = RedColor }, true);
        if (targetUser.TimeOutUntil > DateTimeOffset.UtcNow) return new ValueTuple<EmbedProperties, bool>(new EmbedProperties { Description = "You can't shoot someone who is already muted!", Color = RedColor }, true);

        var embed = new EmbedProperties
        {
            Author = new EmbedAuthorProperties { Name = commandAuthor.Username, IconUrl = commandAuthor.GetAvatarUrl()?.ToString() },
            Footer = new EmbedFooterProperties { Text = $"Current hit chance: {AppDefaults.HitChanceMax}%" }
        };

        var isHit = new Random().Next(0, 100);
        var dbCommandAuthor = await dbContext.Users.FirstOrDefaultAsync(x => x.UserDiscordId == commandAuthor.Id);
        if (dbCommandAuthor == null)
        {
            dbCommandAuthor = new User { UserDiscordId = commandAuthor.Id };
            await dbContext.Users.AddAsync(dbCommandAuthor);
            await dbContext.SaveChangesAsync();
        }

        var dbCommandVictim = await dbContext.Users.FirstOrDefaultAsync(x => x.UserDiscordId == targetUser.Id);
        if (dbCommandVictim == null)
        {
            dbCommandVictim = new User { UserDiscordId = targetUser.Id };
            await dbContext.Users.AddAsync(dbCommandVictim);
            await dbContext.SaveChangesAsync();
        }

        logger.LogDebug("Rolled {Roll} for {User}", isHit, targetUser.Username);
        if (isHit < AppDefaults.HitChanceMax)
        {
            await guild.ModifyUserAsync(targetUser.Id, op => { op.WithTimeOutUntil(DateTimeOffset.UtcNow.AddMinutes(timeInMinutes)); });

            embed.Color = GreenColor;
            embed.Description = $"<@{targetUser.Id}> was hit by <@{commandAuthor.Id}> and has been muted for `{timeInMinutes}m`!";
            dbCommandAuthor.SuccessfulHits++;
            dbCommandAuthor.MinutesMutedOthers += timeInMinutes;
            dbCommandVictim.MinutesMutedByOthers += timeInMinutes;

            await dbContext.SaveChangesAsync();

            return new ValueTuple<EmbedProperties, bool>(embed, false);
        }

        if (triggerRolePos.Any(x => (x.Permissions & Permissions.Administrator) != 0))
        {
            embed.Description = $"<@{commandAuthor.Id}> tried to hit <@{targetUser.Id}> for `{timeInMinutes}m` but missed!\nUnfortunately, they are **immune** to the consequences!";
        }
        else
        {
            await guild.ModifyUserAsync(commandAuthor.Id, op => { op.WithTimeOutUntil(DateTimeOffset.UtcNow.AddMinutes(timeInMinutes)); });
            embed.Description = $"<@{commandAuthor.Id}> tried to hit <@{targetUser.Id}> for `{timeInMinutes}m` but missed!\nFor being a bad shot, they have been timed out!";
            dbCommandAuthor.MinutesMutedSelf += timeInMinutes;
        }

        dbCommandAuthor.HitAttempts++;

        await dbContext.SaveChangesAsync();

        embed.Color = RedColor;
        return new ValueTuple<EmbedProperties, bool>(embed, false);
    }
}