using Discord.Interactions;

namespace DiscordBot.Infrastructure.Configuration;

public class BotInteractionServiceConfig : InteractionServiceConfig
{
    public BotInteractionServiceConfig()
    {
        DefaultRunMode = RunMode.Async;
    }
}