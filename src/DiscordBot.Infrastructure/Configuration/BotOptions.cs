using Microsoft.Extensions.Options;

namespace DiscordBot.Infrastructure.Configuration;

public class BotOptionsProvider : IBotOptions
{
    private readonly IOptions<BotOptions> options;

    public BotOptionsProvider(IOptions<BotOptions> options)
    {
        this.options = options;
    }

    public string Nickname => options.Value.Nickname;
    public string Token => options.Value.Token;
    public ulong? TestGuildId => options.Value.TestGuildId;
}

public class BotOptions : IBotOptions
{
    public const string SectionName = "Bot";

    public string Nickname { get; set; } = "Bot";
    public string Token { get; set; } = string.Empty;
    public ulong? TestGuildId { get; set; }
}

public interface IBotOptions
{
    public string Nickname { get; }
    public string Token { get; }
    public ulong? TestGuildId { get; }
}