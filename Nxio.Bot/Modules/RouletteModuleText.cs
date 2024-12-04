using JetBrains.Annotations;
using NetCord;
using NetCord.Rest;
using NetCord.Services.Commands;
using Nxio.Bot.Modules.Common;
using Nxio.Core.Database;

namespace Nxio.Bot.Modules;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers, Reason = "Registered via module discovery")]
public class RouletteModuleText(ILogger<RouletteModuleText> logger, BaseDbContext dbContext) : CommandModule<CommandContext>
{
    [Command("roulette", "roll", "mute", "r")]
    public async Task<ReplyMessageProperties> Roulette(
        [CommandParameter(Name = "user")] GuildUser targetUser,
        [CommandParameter(Name = "duration")] int timeInMinutes
    )
    {
        logger.LogDebug("Guild: {Guild} | User: {User} ({UserId})", Context.Guild!.Name, Context.User.Username, Context.User.Id);
        if (Context.Guild == null) return new ReplyMessageProperties { Content = "This command can only be used in a guild!" };
        var (embed, _) = await RouletteCommand.Run(logger, dbContext, targetUser, Context.Client, Context.User, timeInMinutes, Context.Guild);
        return new ReplyMessageProperties { Embeds = [embed] };
    }
}