using NetCord.Gateway;
using NetCord.Hosting.Gateway;

namespace Nxio.App.Handlers;

[GatewayEvent(nameof(GatewayClient.MessageReactionAdd))]
public class MessageReactionAdd(ILogger<MessageReactionAdd> logger) : IGatewayEventHandler<MessageReactionAddEventArgs>
{
    public ValueTask HandleAsync(MessageReactionAddEventArgs args)
    {
        logger.LogInformation("{Reaction}", $"{args.User?.Username} reacted {args.Emoji.Name}!");
        return ValueTask.CompletedTask;
    }
}