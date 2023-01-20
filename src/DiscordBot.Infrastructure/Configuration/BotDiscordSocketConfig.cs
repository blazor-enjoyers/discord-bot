using Discord;
using Discord.WebSocket;

namespace DiscordBot.Infrastructure.Configuration;

public class BotDiscordSocketConfig : DiscordSocketConfig
{
    public BotDiscordSocketConfig()
    {
        GatewayIntents = GetGatewayIntents();
    }

    private static GatewayIntents GetGatewayIntents()
    {
        return GatewayIntents.Guilds
            | GatewayIntents.GuildBans
            | GatewayIntents.GuildEmojis
            | GatewayIntents.GuildIntegrations
            //| GatewayIntents.GuildInvites
            //| GatewayIntents.GuildMembers
            | GatewayIntents.GuildMessages
            //| GatewayIntents.GuildPresences
            | GatewayIntents.GuildWebhooks
            | GatewayIntents.GuildMessageReactions
            | GatewayIntents.GuildMessageTyping
            //| GatewayIntents.GuildScheduledEvents
            | GatewayIntents.GuildVoiceStates
            | GatewayIntents.DirectMessages
            | GatewayIntents.DirectMessageReactions
            | GatewayIntents.DirectMessageTyping;
    }
}