using Microsoft.EntityFrameworkCore;
using NetCord.Rest;
using Nxio.Core.Database;
using Nxio.Core.Database.Models.Enums;
using Nxio.Core.Extensions;

namespace Nxio.Bot.BackgroundWorkers;

public class MuteWorker(ILogger<MuteWorker> logger, IServiceProvider serviceProvider, RestClient client) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogDebug("Mute Worker started at: {Time}", DateTimeOffset.UtcNow);
        await Task.Delay(60 * 1_000, stoppingToken); // Delay worker start in order to allow the bot to initialize properly

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogDebug("Mute Worker running at: {Time}", DateTimeOffset.UtcNow);

            await using var scope = serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

            var mutedUsers = await dbContext.UserMutes.Where(x => x.MuteEndUtc < DateTimeOffset.UtcNow).ToListAsync(cancellationToken: stoppingToken);
            if (mutedUsers.Count != 0)
            {
                var mutedUsersGuilds = mutedUsers.Select(x => x.GuildId).Distinct();
                var mutedRolesIds = await dbContext.GuildSettings
                    .AsNoTracking()
                    .Where(x => mutedUsersGuilds.Contains(x.GuildId))
                    .Where(x => x.Name == GuildSettingName.DefaultMuteRole)
                    .ToListAsync(cancellationToken: stoppingToken);

                foreach (var user in mutedUsers)
                {
                    var fetchedUser = await client.GetGuildUserAsync(userId: user.UserId, guildId: user.GuildId, cancellationToken: stoppingToken);
                    if (fetchedUser.IsBot || fetchedUser.IsSystemUser == true) continue;

                    var targetGuildSetting = mutedRolesIds.FirstOrDefault(x => x.GuildId == user.GuildId);
                    if (targetGuildSetting == null)
                    {
                        logger.LogInformation("Unable to remove mute role from user {UserId} in guild {GuildId} - no default mute role set", user.UserId, user.GuildId);
                        continue;
                    }

                    var mutedRoleSettingValue = targetGuildSetting.GetGuildSettingValue<ulong>();
                    var muteRole = fetchedUser.RoleIds.FirstOrDefault(x => x == mutedRoleSettingValue);
                    if (muteRole != 0)
                        await client.RemoveGuildUserRoleAsync(userId: user.UserId, guildId: user.GuildId, roleId: muteRole, cancellationToken: stoppingToken);

                    foreach (var roleId in user.RoleIdsBeforeMute.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)))
                    {
                        if (!ulong.TryParse(roleId, out var roleIdParsed)) continue;
                        logger.LogDebug("Adding role {RoleId} to user {UserId} in guild {GuildId}", roleIdParsed, user.UserId, user.GuildId);
                        await client.AddGuildUserRoleAsync(userId: user.UserId, guildId: user.GuildId, roleId: roleIdParsed, cancellationToken: stoppingToken);
                    }

                    dbContext.UserMutes.Remove(user);
                    await Task.Delay(1_000, stoppingToken);
                }

                if (dbContext.ChangeTracker.HasChanges())
                    await dbContext.SaveChangesAsync(cancellationToken: stoppingToken);
            }

            await Task.Delay(30 * 1_000, stoppingToken);
        }

        logger.LogDebug("Mute Worker stopped at: {Time}", DateTimeOffset.UtcNow);
    }
}