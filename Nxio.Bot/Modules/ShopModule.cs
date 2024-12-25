using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Nxio.Core;
using Nxio.Core.Database;
using Nxio.Core.Database.Models.Enums;
using Nxio.Core.Extensions;

namespace Nxio.Bot.Modules;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers, Reason = "Registered via module discovery")]
public class ShopModule(ILogger<ShopModule> logger, BaseDbContext context) : ApplicationCommandModule<ApplicationCommandContext>
{
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
            Color = new Color(0, 255, 0)
        };

        return new InteractionMessageProperties { Embeds = [embed] };
    }
}