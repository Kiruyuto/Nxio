using NetCord.Gateway;
using NetCord.Hosting.Gateway;

namespace Nxio.Bot.Handlers;

[GatewayEvent(nameof(GatewayClient.MessageCreate))]
public class MessageCreate(ILogger<MessageCreate> logger) : IGatewayEventHandler<Message>
{
    public ValueTask HandleAsync(Message msg)
    {
        logger.LogInformation("{Content}", msg.Content);
        return ValueTask.CompletedTask;
    }
}