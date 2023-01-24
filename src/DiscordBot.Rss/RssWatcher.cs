using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Rss;

public class RssWatcher : IRssWatcher, IDisposable
{
    private readonly IClock clock;
    private readonly CancellationTokenSource cts = new();
    private readonly IRssFeedsRegistry feedsRegistry;
    private readonly ILogger<RssWatcher> logger;
    private readonly IRssQueue queue;
    private DateTime lastRefreshTime;

    public RssWatcher(IRssQueue queue, IRssFeedsRegistry feedsRegistry, ILogger<RssWatcher> logger, IClock clock)
    {
        this.queue = queue;
        this.feedsRegistry = feedsRegistry;
        this.logger = logger;
        this.clock = clock;
        lastRefreshTime = clock.UtcNow;
    }

    public void Dispose()
    {
        cts.Dispose();
    }

    public async Task RefreshAsync(CancellationToken ct = default)
    {
        foreach (var feedUrl in feedsRegistry.Urls)
        {
            try
            {
                if (cts.IsCancellationRequested || ct.IsCancellationRequested)
                    return;

                await RefreshFeedAsync(feedUrl, lastRefreshTime, ct);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error during refreshing feed {Url}", feedUrl);
            }
        }

        lastRefreshTime = clock.UtcNow;
    }

    public async Task AddAsync(string feedUrl, CancellationToken ct = default)
    {
        feedsRegistry.Add(feedUrl);
        await RefreshFeedAsync(feedUrl, clock.UtcNow, ct);
    }

    private async Task RefreshFeedAsync(string feedUrl, DateTime fromTime, CancellationToken ct = default)
    {
        var xmlReader = XmlReader.Create(feedUrl);
        var feed = SyndicationFeed.Load(xmlReader);
        var newItems = feed.Items.Where(i => i.PublishDate > fromTime);
        foreach (var item in newItems)
        {
            if (cts.IsCancellationRequested || ct.IsCancellationRequested)
                return;

            await queue.AddAsync(item.Title.Text);
        }
    }
}

public interface IRssWatcher
{
    Task RefreshAsync(CancellationToken ct = default);
    Task AddAsync(string feedUrl, CancellationToken ct = default);
}