using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Nxio.Bot.Modules;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers, Reason = "Registered via module discovery")]
public class RouletteModule(ILogger<RouletteModule> logger) : ApplicationCommandModule<ApplicationCommandContext>
{
    private const int HitChanceMax = 15;

    [SlashCommand("roulette", "Play a discord version of Russian Roulette!", Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> Roulette(
        [SlashCommandParameter(Name = "user", Description = "Target user")] GuildUser targetUser,
        [SlashCommandParameter(Name = "duration", Description = "Duration to timeout target, in minutes", MinValue = 1, MaxValue = 15)]
        int timeInMinutes
    )
    {
        logger.LogDebug("Guild: {Guild} | User: {User} ({UserId})", Context.Guild!.Name, Context.User.Username, Context.User.Id);

        if (targetUser.Id == Context.User.Id) return new InteractionMessageProperties { Content = "You can't shoot yourself", Flags = MessageFlags.Ephemeral };
        if (targetUser.IsBot) return new InteractionMessageProperties { Content = "You can't shoot a bot!", Flags = MessageFlags.Ephemeral };

        var botRoles = (await Context.Guild!.GetUserAsync(Context.Client.Id)).GetRoles(Context.Guild).ToList();
        var triggerRolePos = (await Context.Guild!.GetUserAsync(Context.User.Id)).GetRoles(Context.Guild);
        if (!botRoles.Any(x => x.Position >= triggerRolePos.Max(z => z.Position)))
            return new InteractionMessageProperties { Content = "You can't play this game because you have higher permissions than me!", Flags = MessageFlags.Ephemeral };

        var targetRoles = targetUser.GetRoles(Context.Guild).ToList();
        // if (targetRoles.Any(x => x.Position > triggerRolePos)) return "You can't shoot someone with a higher role than you!";
        if (targetRoles.Any(x => (x.Permissions & Permissions.Administrator) != 0)) return new InteractionMessageProperties { Content = "You can't shoot an admin!", Flags = MessageFlags.Ephemeral };
        if (targetUser.TimeOutUntil > DateTimeOffset.UtcNow) return new InteractionMessageProperties { Content = "You can't shoot someone who is already muted!", Flags = MessageFlags.Ephemeral };

        var embed = new EmbedProperties
        {
            Author = new EmbedAuthorProperties { Name = Context.User.Username, IconUrl = Context.User.GetAvatarUrl()?.ToString() },
            Footer = new EmbedFooterProperties { Text = $"Current hit chance: {HitChanceMax}%" }
        };

        var isHit = new Random().Next(0, 100);
        logger.LogDebug("Rolled {Roll} for {User}", isHit, targetUser.Username);
        if (isHit < HitChanceMax)
        {
            if (Context.Guild == null) return new InteractionMessageProperties { Content = "Fetching context failed!\nPlease report it to the author!", Flags = MessageFlags.Ephemeral };
            await Context.Guild.ModifyUserAsync(targetUser.Id, op => { op.WithTimeOutUntil(DateTimeOffset.UtcNow.AddMinutes(timeInMinutes)); });

            embed.Color = new Color(0, 255, 0);
            embed.Description = $"<@{targetUser.Id}> was hit by <@{Context.User.Id}> and has been muted for {timeInMinutes} minutes!";

            return new InteractionMessageProperties { Embeds = [embed] };
        }

        if (triggerRolePos.Any(x => (x.Permissions & Permissions.Administrator) != 0))
        {
            embed.Description = $"<@{Context.User.Id}> tried to hit <@{targetUser.Id}> but missed!\nUnfortunately, they are **immune** to the consequences!";
        }
        else
        {
            await Context.Guild!.ModifyUserAsync(Context.User.Id, op => { op.WithTimeOutUntil(DateTimeOffset.UtcNow.AddMinutes(timeInMinutes)); });
            embed.Description = $"<@{Context.User.Id}> tried to hit <@{targetUser.Id}> but missed!\nFor being a bad shot, they have been muted for {timeInMinutes} minutes!";
        }

        embed.Color = new Color(255, 0, 0);
        return new InteractionMessageProperties { Embeds = [embed] };
    }

    [SlashCommand("test-luck", "Test your luck!", Contexts = [InteractionContextType.Guild])]
    public InteractionMessageProperties TestLuck([SlashCommandParameter(Name = "num", Description = "Times to generate", MinValue = 1, MaxValue = 20)] int num)
    {
        var str = new StringBuilder();

        var hits = 0;
        for (var i = 0; i < num; i++)
        {
            var isHit = new Random().Next(0, 100);
            if (isHit < HitChanceMax) hits++;
            str.AppendLine(CultureInfo.InvariantCulture, $"Attempt {i + 1}: {(isHit < HitChanceMax ? "**Hit**" : "Miss")} (Rolled: {isHit})\n");
        }

        var embed = new EmbedProperties
        {
            Title = $"Total hits: {hits}/{num}",
            Description = str.ToString(),
            Color = new Color(0, 255, 0),
            Footer = new EmbedFooterProperties { Text = $"Current hit chance: {HitChanceMax}%" }
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
}