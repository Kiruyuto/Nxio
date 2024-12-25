using JetBrains.Annotations;
using NetCord;
using NetCord.Rest;
using NetCord.Services.Commands;
using Nxio.Bot.Modules.Common;
using Nxio.Core.Database;

namespace Nxio.Bot.Modules;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers, Reason = "Registered via module discovery")]
public class MuteModuleText(ILogger<MuteModuleText> logger, BaseDbContext dbContext) : CommandModule<CommandContext>
{
    [Command("mute", "m")]
    public async Task<ReplyMessageProperties> Mute(
        [CommandParameter(Name = "user")] GuildUser targetUser,
        [CommandParameter(Name = "duration")] int timeInMinutes, // TODO" Rewrite to accept 12h, 1d, 1w, etc. syntax
        [CommandParameter(Name = "reason", Remainder = true)] string reason = "No reason provided"
    )
    {
        logger.LogDebug("Guild: {Guild} | User: {User} ({UserId})", Context.Guild!.Name, Context.User.Username, Context.User.Id);
        if (Context.Guild == null) return new ReplyMessageProperties { Content = "This command can only be used in a guild!" };
        var (embed, _) = await MuteCommand.Run(logger, dbContext, targetUser, Context.Client, Context.User, timeInMinutes, reason, Context.Guild);
        return new ReplyMessageProperties { Embeds = [embed] };
    }
}