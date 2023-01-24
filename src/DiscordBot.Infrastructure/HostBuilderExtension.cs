using System.Reflection;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot.Infrastructure.Commands;
using DiscordBot.Infrastructure.Configuration;
using DiscordBot.Infrastructure.Logging;
using DiscordBot.Infrastructure.Rss;
using DiscordBot.Rss;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DiscordBot.Infrastructure;

public static class HostBuilderExtension
{
    public static IHostBuilder UseDiscordBot(this IHostBuilder hostBuilder, string[] args)
    {
        hostBuilder
            .ConfigureHostConfiguration(args)
            .ConfigureAppConfiguration()
            .ConfigureServices()
            .UseSerilog();

        return hostBuilder;
    }

    private static IHostBuilder ConfigureHostConfiguration(this IHostBuilder hostBuilder, string[] args)
    {
        return hostBuilder.ConfigureHostConfiguration(config =>
        {
            config
                .AddEnvironmentVariables()
                .AddCommandLine(args);
        });
    }

    private static IHostBuilder ConfigureAppConfiguration(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureAppConfiguration((context, builder) =>
        {
            var exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (exeDirectory is not null)
                builder.SetBasePath(exeDirectory);

            builder
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
        });
    }

    private static IHostBuilder ConfigureServices(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((context, collection) =>
        {
            collection
                .AddLogging()
                .AddSingleton<IClock, Clock>()
                .AddRssWatcher()
                .AddBotServices(context);
        });
    }

    private static IServiceCollection AddBotServices(this IServiceCollection collection, HostBuilderContext context)
    {
        return collection
            .Configure<BotOptions>(context.Configuration.GetSection(BotOptions.SectionName))
            .AddSingleton<IBotOptions, BotOptionsProvider>()
            .AddSingleton<DiscordSocketConfig, BotDiscordSocketConfig>()
            .AddSingleton<InteractionServiceConfig, BotInteractionServiceConfig>()
            .AddSingleton<InteractionService>()
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<IBotVersionProvider, BotVersionProvider>()
            .AddSingleton<LoggingHandler>()
            .AddSingleton<CommandHandler>()
            .AddHostedService<BotService>()
            .AddHostedService<RssUpdateService>();
    }
}