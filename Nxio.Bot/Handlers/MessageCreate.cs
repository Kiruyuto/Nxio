using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Rest;
using Nxio.Core.Database;
using Nxio.Core.Database.Models;

namespace Nxio.Bot.Handlers;

[GatewayEvent(nameof(GatewayClient.MessageCreate))]
public class MessageCreate(ILogger<MessageCreate> logger, BaseDbContext context) : IGatewayEventHandler<Message>
{
    private const int CoinAppearChance = 5;

    public async ValueTask HandleAsync(Message msg)
    {
        if (msg.Type != MessageType.Default) return;
        if (msg.Author.IsBot || msg.Author.IsSystemUser == true) return;
        if (msg.GuildId == null) return;

        var roll = new Random().Next(0, 100);
        logger.LogDebug("Rolled: {Roll}", roll);
        if (roll < CoinAppearChance)
        {
            await msg.AddReactionAsync(new ReactionEmojiProperties("coin", 1312832564788068512));
            await context.CoinMessages.AddAsync(new CoinMessage
            {
                GuildId = msg.GuildId!.Value,
                MessageId = msg.Id
            });

            await context.SaveChangesAsync();
        }
    }
}