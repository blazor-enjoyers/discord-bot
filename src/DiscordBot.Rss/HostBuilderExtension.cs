using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Rss;

public static class HostBuilderExtension
{
    public static IServiceCollection AddRssWatcher(this IServiceCollection collection)
    {
        return collection
            .AddSingleton<IRssFeedsRegistry, RssFeedsRegistry>()
            .AddSingleton<IRssQueue, RssQueue>()
            .AddSingleton<IRssWatcher, RssWatcher>()
            .AddHostedService<RssWatcherService>();
    }
}