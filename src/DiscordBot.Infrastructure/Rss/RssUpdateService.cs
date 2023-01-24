using DiscordBot.Hosting;
using DiscordBot.Rss;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Infrastructure.Rss;

public class RssUpdateService : BackgroundServiceWithLogging
{
    private readonly IRssQueue queue;

    public RssUpdateService(ILogger<BackgroundServiceWithLogging> logger, IRssQueue queue) : base(logger)
    {
        this.queue = queue;
    }

    protected override async Task ExecuteServiceAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in queue.ReadAllAsync(stoppingToken))
        {
            Console.WriteLine(item);
        }
    }
}