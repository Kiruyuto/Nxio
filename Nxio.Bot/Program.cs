﻿using Microsoft.EntityFrameworkCore;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Services.ApplicationCommands;
using Nxio.Core.Database;

namespace Nxio.Bot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services
            .AddDbContext<BaseDbContext>(op =>
            {
                var connStr = builder.Configuration.GetConnectionString("DbConnection");
                if (string.IsNullOrWhiteSpace(connStr)) throw new ArgumentNullException(paramName: nameof(connStr), message: "DbConnection not set in configuration!");

                op.UseSqlServer(connectionString: connStr);
#if DEBUG
                op.EnableSensitiveDataLogging();
#endif
            });

        builder.Services
            .AddGatewayEventHandlers(typeof(Program).Assembly)
            .AddApplicationCommands<ApplicationCommandInteraction, ApplicationCommandContext>()
            .AddDiscordGateway(op =>
            {
                op.Intents = GatewayIntents.All; // TODO: Use explicit intents instead
            });

        var host = builder
            .Build()
            .AddModules(typeof(Program).Assembly)
            .UseGatewayEventHandlers();

        await host.RunAsync();
    }
}