using DiscordBot.Infrastructure;
using Microsoft.Extensions.Hosting;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var app = CreateHostBuilder(args).Build();
await app.RunAsync();

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .UseSystemd()
        .UseWindowsService()
        .UseDiscordBot(args);
}