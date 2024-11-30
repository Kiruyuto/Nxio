using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Rest;

namespace Nxio.App.Handlers;

[GatewayEvent(nameof(GatewayClient.MessageReactionAdd))]
public class MessageReactionAdd(RestClient client, ILogger<MessageReactionAdd> logger) : IGatewayEventHandler<MessageReactionAddEventArgs>
{
    public ValueTask HandleAsync(MessageReactionAddEventArgs args)
    {
        logger.LogInformation("{Reaction}", $"<@{args.UserId}> reacted {args.Emoji.Name}!");
        return ValueTask.CompletedTask;
    }
}