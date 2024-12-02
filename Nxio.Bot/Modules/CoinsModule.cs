using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Nxio.Core.Database;

namespace Nxio.Bot.Modules;

public class CoinsModule(ILogger<CoinsModule> logger, IServiceProvider serviceProvider) : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("coins", "Check your or someones else coin balance!", Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> Coins(
        [SlashCommandParameter(Name = "user", Description = "Target user. Checks self balance if not provided.")] GuildUser? targetUser)
    {
        logger.LogDebug("Guild: {Guild} | User: {User} ({UserId})", Context.Guild!.Name, Context.User.Username, Context.User.Id);

        var user = targetUser ?? Context.User;

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

        var usr = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserDiscordId == user.Id);
        return usr == null
            ? new InteractionMessageProperties { Content = "User has no coins!", Flags = MessageFlags.Ephemeral }
            : new InteractionMessageProperties { Content = $"<@{user.Id}> has [**{usr.Coins}**] coins!" };
    }

    [SlashCommand("coins-leaderboard", "List users with most coins on the server!", Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> CoinsLeaderboard()
    {
        logger.LogDebug("Guild: {Guild} | User: {User} ({UserId})", Context.Guild!.Name, Context.User.Username, Context.User.Id);

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

        var usrLst = await context.Users.AsNoTracking().OrderByDescending(x => x.Coins).Take(10).ToListAsync();
        if (usrLst.Count == 0) return new InteractionMessageProperties { Content = "No users found!", Flags = MessageFlags.Ephemeral };
        var userWithCoinsCount = await context.Users.AsNoTracking().CountAsync(x => x.Coins > 0);

        var stringBuilder = new StringBuilder();

        for (var i = 0; i < usrLst.Count; i++)
        {
            var u = usrLst[i];
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{i + 1}. <@{u.UserDiscordId}> - **{u.Coins}** coins\n");
        }

        var embed = new EmbedProperties
        {
            Author = new EmbedAuthorProperties { Name = Context.User.Username, IconUrl = Context.User.GetAvatarUrl()?.ToString() },
            Description = stringBuilder.ToString(),
            Color = new Color(0, 255, 0),
            Footer = new EmbedFooterProperties { Text = $"Users with 1+ coin: {userWithCoinsCount}" }
        };

        return new InteractionMessageProperties { Embeds = [embed] };
    }
}