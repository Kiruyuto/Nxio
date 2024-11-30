using NetCord.Gateway;
using NetCord.Hosting.Gateway;

namespace Nxio.App;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddDiscordGateway(op =>
        {
            op.Intents = GatewayIntents.All; // TODO: Use explicit intents instead
        }).AddGatewayEventHandlers(typeof(Program).Assembly);

        var host = builder.Build().UseGatewayEventHandlers();

        await host.RunAsync();
    }
}