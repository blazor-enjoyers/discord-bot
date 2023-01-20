using System.Reflection;
using Discord.WebSocket;
using DiscordBot.Infrastructure.Configuration;
using DiscordBot.Infrastructure.Logging;
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
            config.AddEnvironmentVariables();
            config.AddCommandLine(args);
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
            collection.AddLogging();
            collection.AddBotServices(context);
        });
    }

    private static IServiceCollection AddBotServices(this IServiceCollection collection, HostBuilderContext context)
    {
        collection.Configure<BotOptions>(context.Configuration.GetSection(BotOptions.SectionName));
        collection.AddSingleton<DiscordSocketConfig, BotDiscordSocketConfig>();
        collection.AddSingleton<IBotOptions, BotOptionsProvider>();
        collection.AddSingleton<DiscordSocketClient>();
        collection.AddSingleton<IBotVersionProvider, BotVersionProvider>();
        collection.AddSingleton<LoggingHandler>();
        collection.AddHostedService<BotService>();

        return collection;
    }
}