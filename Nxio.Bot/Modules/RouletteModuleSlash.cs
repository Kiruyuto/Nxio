using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Nxio.Bot.Modules.Common;
using Nxio.Core;
using Nxio.Core.Database;

namespace Nxio.Bot.Modules;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers, Reason = "Registered via module discovery")]
public class RouletteModuleSlash(ILogger<RouletteModuleSlash> logger, BaseDbContext dbContext) : ApplicationCommandModule<ApplicationCommandContext>
{
    private const int AverageGameLength = 30; // https://www.leagueofgraphs.com/stats/game-durations

    [SlashCommand("roulette", "Play a discord version of Russian Roulette!", Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> Roulette(
        [SlashCommandParameter(Name = "user", Description = "Target user")] GuildUser targetUser,
        [SlashCommandParameter(Name = "duration", Description = "Duration to timeout target, in minutes", MinValue = RouletteCommand.MinValue, MaxValue = RouletteCommand.MaxValue)]
        int timeInMinutes
    )
    {
        if (Context.Guild == null) return new InteractionMessageProperties { Content = "This command can only be used in a guild!", Flags = MessageFlags.Ephemeral };
        var (embed, ephemeral) = await RouletteCommand.Run(logger, dbContext, targetUser, Context.Client, Context.User, timeInMinutes, Context.Guild);
        return new InteractionMessageProperties { Embeds = [embed], Flags = ephemeral ? MessageFlags.Ephemeral : null, };
    }

    [SlashCommand("test-luck", "Test your luck!", Contexts = [InteractionContextType.Guild])]
    public InteractionMessageProperties TestLuck([SlashCommandParameter(Name = "num", Description = "Times to generate", MinValue = 1, MaxValue = 20)] int num)
    {
        var str = new StringBuilder();

        var hits = 0;
        for (var i = 0; i < num; i++)
        {
            var isHit = new Random().Next(0, 100);
            if (isHit < AppDefaults.HitChanceMax) hits++;
            str.AppendLine(CultureInfo.InvariantCulture, $"Attempt {i + 1}: {(isHit < AppDefaults.HitChanceMax ? "**Hit**" : "Miss")} (Rolled: {isHit})\n");
        }

        var embed = new EmbedProperties
        {
            Title = $"Total hits: {hits}/{num}",
            Description = str.ToString(),
            Color = new Color(0, 255, 0),
            Footer = new EmbedFooterProperties { Text = $"Current hit chance: {AppDefaults.HitChanceMax}%" }
        };

        return new InteractionMessageProperties { Embeds = [embed], Flags = MessageFlags.Ephemeral };
    }

    [SlashCommand("list-timeouts", "Display all currently muted users!", Contexts = [InteractionContextType.Guild])]
    public InteractionMessageProperties GetAllTimeouts()
    {
        const int take = 30;
        var users = Context.Guild!.Users
            .Where(static x => x.Value.TimeOutUntil != null && x.Value.TimeOutUntil > DateTimeOffset.UtcNow)
            .OrderByDescending(static x => x.Value.TimeOutUntil)
            .Take(take)
            .ToList();
        if (users.Count == 0) return new InteractionMessageProperties { Content = "No users are currently muted!", Flags = MessageFlags.Ephemeral };


        var str = new StringBuilder();
        foreach (var u in users.Select(static x => x.Value))
            str.AppendLine(CultureInfo.InvariantCulture, $"<@{u.Id}> - Expires <t:{u.TimeOutUntil!.Value.ToUnixTimeSeconds()}:R>\n");

        var embed = new EmbedProperties
        {
            Title = $"Currently muted: {users.Count}",
            Description = str.ToString(),
            Color = new Color(0, 255, 0),
            Footer = new EmbedFooterProperties { Text = $"Displaying only first {take} records" }
        };

        return new InteractionMessageProperties { Embeds = [embed] };
    }

    [SlashCommand("stats", "Display user stats!", Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> GetUserStats(
        [SlashCommandParameter(Name = "user", Description = "Target user")] GuildUser? targetUser = null
    )
    {
        var user = targetUser ?? Context.User;
        var dbUser = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserDiscordId == user.Id);
        if (dbUser == default) return new InteractionMessageProperties { Content = "User has no stats!", Flags = MessageFlags.Ephemeral };

        var timeMutedByOthers = TimeSpan.FromMinutes(dbUser.MinutesMutedByOthers);
        var timeMutedOthers = TimeSpan.FromMinutes(dbUser.MinutesMutedOthers);
        var timeMutedSelf = TimeSpan.FromMinutes(dbUser.MinutesMutedSelf);

        var embed = new EmbedProperties
        {
            Title = "Stats panel",
            Description = $"<@{user.Id}>\nAll times are rounded to the nearest minute.",
            Color = new Color(0, 255, 0),
            Fields =
            [
                new EmbedFieldProperties { Name = "Hit Attempts", Value = $"{dbUser.HitAttempts}", Inline = true },
                new EmbedFieldProperties { Name = "Hit Success", Value = $"{dbUser.SuccessfulHits}", Inline = true },
                new EmbedFieldProperties { Name = "Hit Rate", Value = $"{double.Round((double)dbUser.SuccessfulHits / dbUser.HitAttempts, 2, MidpointRounding.AwayFromZero)}%", Inline = true },

                new EmbedFieldProperties { Name = "Time spent muted by others", Value = $"{timeMutedByOthers.Hours}h {timeMutedByOthers.Minutes}m", Inline = true },
                new EmbedFieldProperties { Name = "Time others have spent on mute", Value = $"{timeMutedOthers.Hours}h {timeMutedOthers.Minutes}m", Inline = true },
                new EmbedFieldProperties { Name = "Time spent self muted", Value = $"{timeMutedSelf.Hours}h {timeMutedSelf.Minutes}m", Inline = true }
            ],
            Footer = new EmbedFooterProperties
            {
                Text = $"{user.GlobalName} has spent ~{double.Round((double)dbUser.MinutesMutedSelf / AverageGameLength, 1, MidpointRounding.AwayFromZero)} Emerald ELO League Of Legends games sitting on timeout "
            }
        };

        return new InteractionMessageProperties { Embeds = [embed] };
    }
}