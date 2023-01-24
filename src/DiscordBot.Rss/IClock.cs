namespace DiscordBot.Rss;

public interface IClock
{
    DateTime UtcNow { get; }
}