using Microsoft.EntityFrameworkCore;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.Commands;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;
using Nxio.Bot.BackgroundWorkers;
using Nxio.Core.Database;

namespace Nxio.Bot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services
            .AddHostedService<MuteWorker>()
            .AddGatewayEventHandlers(typeof(Program).Assembly)
            .AddApplicationCommands<ApplicationCommandInteraction, ApplicationCommandContext>(op => op.ResultHandler = new ApplicationCommandResultHandler<ApplicationCommandContext>(MessageFlags.Ephemeral))
            .AddCommands<CommandContext>(op => op.IgnoreCase = true)
            .AddDiscordGateway(op =>
            {
                op.Intents = GatewayIntents.All; // TODO: Use explicit intents instead
            })
            .AddDbContext<BaseDbContext>(op =>
            {
                var connStr = builder.Configuration.GetConnectionString("Database");
                if (string.IsNullOrWhiteSpace(connStr)) throw new ArgumentNullException(paramName: nameof(connStr), message: "DbConnection not set in configuration!");

                op.UseSqlServer(connectionString: connStr);
#if DEBUG
                op.EnableSensitiveDataLogging();
#endif
            });

        var host = builder
            .Build()
            .AddModules(typeof(Program).Assembly)
            .UseGatewayEventHandlers();

        await host.RunAsync();
    }
}