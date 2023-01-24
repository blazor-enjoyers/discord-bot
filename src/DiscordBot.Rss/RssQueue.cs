using System.Threading.Channels;

namespace DiscordBot.Rss;

public class RssQueue : IRssQueue
{
    private readonly Channel<string> updatesChannel = Channel.CreateUnbounded<string>();

    public async Task AddAsync(string update)
    {
        await updatesChannel.Writer.WriteAsync(update);
    }

    public IAsyncEnumerable<string> ReadAllAsync(CancellationToken cancellationToken = default)
        => updatesChannel.Reader.ReadAllAsync(cancellationToken);
}

public interface IRssQueue
{
    Task AddAsync(string update);
    IAsyncEnumerable<string> ReadAllAsync(CancellationToken cancellationToken = default);
}