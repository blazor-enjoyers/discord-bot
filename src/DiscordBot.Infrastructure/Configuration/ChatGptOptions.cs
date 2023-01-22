using Microsoft.Extensions.Options;

namespace DiscordBot.Infrastructure.Configuration;

public class ChatGptOptionsProvider : IChatGptOptions
{
    private readonly IOptions<ChatGptOptions> _options;
    
    public ChatGptOptionsProvider(IOptions<ChatGptOptions> options)
    {
        _options = options;
    }

    public string BearerToken => _options.Value.BearerToken;
}

public class ChatGptOptions : IChatGptOptions
{
    public const string SectionName = "ChaptGpt";
    public string BearerToken { get; set; } = string.Empty;
}

public interface IChatGptOptions
{
    public string BearerToken { get; }
}