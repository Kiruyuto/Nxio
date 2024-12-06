using JetBrains.Annotations;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Rest;

namespace Nxio.Bot.Handlers;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers, Reason = "Registered via GatewayEventHandler discovery")]
[GatewayEvent(nameof(GatewayClient.Ready))]
public class Ready(ILogger<Ready> logger) : IGatewayEventHandler<ReadyEventArgs>
{
    public async ValueTask HandleAsync(ReadyEventArgs arg)
    {
        logger.LogInformation("User: {User} ({UserId})", arg.User.Username, arg.User.Id);
        var members = arg.User.GetGuildsAsync(new GuildsPaginationProperties { WithCounts = true });
        var membersCount = 0;
        await foreach (var m in members) membersCount += m.ApproximateUserCount ?? 0;

        logger.LogInformation("Working on {GuildsCount} server with approximately {GuildsUserCount} members using API {ApiVersion}", arg.GuildIds.Count, membersCount, arg.Version);
    }
}