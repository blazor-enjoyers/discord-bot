using System.Collections.Concurrent;

namespace DiscordBot.Rss;

public class RssFeedsRegistry : IRssFeedsRegistry
{
     private readonly ConcurrentBag<string> bag = new();

     public void Add(string url)
     {
          bag.Add(url);
     }

     public IReadOnlyCollection<string> Urls => bag.ToArray();
}

public interface IRssFeedsRegistry
{
     void Add(string url);
     IReadOnlyCollection<string> Urls { get; }
}