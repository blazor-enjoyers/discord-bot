using DiscordBot.Rss;

namespace DiscordBot.Infrastructure;

public class Clock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}