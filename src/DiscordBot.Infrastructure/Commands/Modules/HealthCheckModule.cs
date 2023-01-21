using Discord.Interactions;

namespace DiscordBot.Infrastructure.Commands.Modules;

public class HealthCheckModule : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("ping", "Check if bot is responding")]
    public async Task Ping() => await RespondAsync("pong");
}