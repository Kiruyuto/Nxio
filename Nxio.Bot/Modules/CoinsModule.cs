using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Nxio.Core;
using Nxio.Core.Database;
using Nxio.Core.Database.Models.Enums;

namespace Nxio.Bot.Modules;

public class CoinsModule(ILogger<CoinsModule> logger, BaseDbContext context) : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("coins", "Check your or another user coin balance!", Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> Coins(
        [SlashCommandParameter(Name = "user", Description = "Target user. Checks self balance if not provided.")] GuildUser? targetUser = null
    )
    {
        logger.LogDebug("Guild: {Guild} | User: {User} ({UserId}) | Command: {Command}", Context.Guild!.Name, Context.User.Username, Context.User.Id, nameof(Coins));

        var user = targetUser ?? Context.User;

        var dbUser = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserDiscordId == user.Id);
        return dbUser == null
            ? new InteractionMessageProperties { Content = "User has no coins!", Flags = MessageFlags.Ephemeral }
            : new InteractionMessageProperties { Content = $"<@{user.Id}> has [**{dbUser.Coins}**] coins", Flags = MessageFlags.Ephemeral };
    }

    [SlashCommand("coins-leaderboard", "List users with most coins on the server!", Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> CoinsLeaderboard()
    {
        logger.LogDebug("Guild: {Guild} | User: {User} ({UserId}) | Command: {Command}", Context.Guild!.Name, Context.User.Username, Context.User.Id, nameof(CoinsLeaderboard));

        var usrLst = await context.Users.AsNoTracking().OrderByDescending(x => x.Coins).Take(10).ToListAsync();
        if (usrLst.Count == 0) return new InteractionMessageProperties { Content = "No users found!", Flags = MessageFlags.Ephemeral };
        var userWithCoinsCount = await context.Users.AsNoTracking().CountAsync(x => x.Coins > 0);

        var stringBuilder = new StringBuilder();

        foreach (var u in usrLst)
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"<@{u.UserDiscordId}> - **{u.Coins}** coins\n");

        var embed = new EmbedProperties
        {
            Title = "Top 10 users with most coins",
            Description = stringBuilder.ToString(),
            Color = new Color(0, 255, 0),
            Footer = new EmbedFooterProperties { Text = $"Users with 1+ coin: {userWithCoinsCount}" },
        };

        return new InteractionMessageProperties { Embeds = [embed] };
    }

    [SlashCommand("check-upgrades", "Check upgrades user has bought!", Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> CheckUpgrades(
        [SlashCommandParameter(Name = "user", Description = "Target user. Checks self upgrades if not provided.")] GuildUser? targetUser = null
    )
    {
        logger.LogDebug("Guild: {Guild} | User: {User} ({UserId}) | Command: {Command}", Context.Guild!.Name, Context.User.Username, Context.User.Id, nameof(CheckUpgrades));

        var user = targetUser ?? Context.User;

        var dbUser = await context.Users.AsNoTracking().Include(x => x.Upgrades).FirstOrDefaultAsync(x => x.UserDiscordId == user.Id);
        if (dbUser == null || dbUser.Upgrades.Count == 0) return new InteractionMessageProperties { Content = "User has no upgrades!", Flags = MessageFlags.Ephemeral };
        var upgradeCount = await context.Upgrades.CountAsync();

        var embed = new EmbedProperties
        {
            Author = new EmbedAuthorProperties { Name = user.Username, IconUrl = user.GetAvatarUrl()?.ToString() },
            Description = $"<@{user.Id}> has {dbUser.Upgrades.Count}/{upgradeCount} upgrades.\n" +
                          $"Total **Hit%** value: `+{dbUser.Upgrades.Where(x => x.Type == UpgradeType.PercentageIncrease).Sum(x => x.ValuePerLevel * x.Level)}%`\n" +
                          $"Total **Res%** value: `+{dbUser.Upgrades.Where(x => x.Type == UpgradeType.PercentageResistance).Sum(x => x.ValuePerLevel * x.Level)}%`",
            Fields = dbUser.Upgrades.Select(x => new EmbedFieldProperties
            {
                Name = $"{x.Name}",
                Value = $"Type: `{x.Type.GetEnumMemberName()}`\nLevel: `{x.Level}`/`{x.MaxLevel}`\nValueTotal: `{x.ValuePerLevel * x.Level}`\nValuePerLevel: `{x.ValuePerLevel}`\n",
                Inline = true
            }).Take(25).ToList(), // TODO: Add paging if there are more than 25 upgrades
            Color = new Color(0, 255, 0),
        };

        return new InteractionMessageProperties { Embeds = [embed] };
    }
}