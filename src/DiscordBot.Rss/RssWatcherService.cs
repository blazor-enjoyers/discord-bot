using DiscordBot.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Rss;

public class RssWatcherService : BackgroundServiceWithLogging
{
    private readonly IRssWatcher watcher;
    private readonly PeriodicTimer timer = new(TimeSpan.FromSeconds(1));

    public RssWatcherService(IRssWatcher watcher, ILogger<RssWatcherService> logger) : base(logger)
    {
        this.watcher = watcher;
    }

    protected override async Task ExecuteServiceAsync(CancellationToken stoppingToken)
    {
        while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            await watcher.RefreshAsync(stoppingToken);
    }
}