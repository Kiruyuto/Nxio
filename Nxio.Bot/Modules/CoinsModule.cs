using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Nxio.Core.Database;

namespace Nxio.Bot.Modules;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers, Reason = "Registered via module discovery")]
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
            Footer = new EmbedFooterProperties { Text = $"Users with 1+ coin: {userWithCoinsCount}" }
        };

        return new InteractionMessageProperties { Embeds = [embed] };
    }
}