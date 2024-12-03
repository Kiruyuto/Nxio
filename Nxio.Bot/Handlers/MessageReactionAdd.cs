using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using Nxio.Core.Database;
using Nxio.Core.Database.Models;

namespace Nxio.Bot.Handlers;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers, Reason = "Registered via GatewayEventHandler discovery")]
[GatewayEvent(nameof(GatewayClient.MessageReactionAdd))]
public class MessageReactionAdd(ILogger<MessageReactionAdd> logger, IServiceProvider serviceProvider) : IGatewayEventHandler<MessageReactionAddEventArgs>
{
    public async ValueTask HandleAsync(MessageReactionAddEventArgs args)
    {
        logger.LogDebug("Reaction: {Emoji}, User: {User}, GuildId: {GuildId}, MessageId: {MessageId}", args.Emoji.Name, args.User?.Username, args.GuildId, args.MessageId);
        if (args.Emoji.Id != 1312832564788068512) return;
        if (args.User == null || args.User.IsBot) return;
        if (args.GuildId == null) return;

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

        var coinMsg = await context.CoinMessages.FirstOrDefaultAsync(x => x.MessageId == args.MessageId);
        if (coinMsg == null) return;

        var usr = await context.Users
            .Include(x => x.CoinReactions)
            .ThenInclude(coinReaction => coinReaction.CoinMessage)
            .FirstOrDefaultAsync(x => x.UserDiscordId == args.User.Id);
        if (usr == null)
        {
            usr = new User
            {
                UserDiscordId = args.User.Id,
                Coins = 1,
                CoinReactions =
                [
                    new CoinReaction
                    {
                        CoinMessage = coinMsg,
                        User = usr!
                    }
                ]
            };

            await context.Users.AddAsync(usr);
            await context.SaveChangesAsync();
        }
        else
        {
            if (usr.CoinReactions.Any(x => x.CoinMessage.MessageId == args.MessageId))
                return;

            usr.Coins++;
            usr.CoinReactions.Add(new CoinReaction { CoinMessage = coinMsg, User = usr });
        }

        if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
    }
}