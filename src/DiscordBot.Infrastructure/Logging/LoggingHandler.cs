using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Infrastructure.Logging;

public class LoggingHandler
{
    private readonly DiscordSocketClient discordClient;
    private readonly ILogger<LoggingHandler> logger;

    public LoggingHandler(DiscordSocketClient discordClient, ILogger<LoggingHandler> logger)
    {
        this.discordClient = discordClient;
        this.logger = logger;
    }

    public void Initialize()
    {
        discordClient.Log += LogAsync;
    }

    private Task LogAsync(LogMessage message)
    {
        switch (message.Severity)
        {
            case LogSeverity.Critical:
                logger.LogCritical(message.Exception, message.Message);
                break;
            case LogSeverity.Error:
                logger.LogError(message.Exception, message.Message);
                break;
            case LogSeverity.Warning:
                logger.LogWarning(message.Exception, message.Message);
                break;
            case LogSeverity.Info:
                logger.LogInformation(message.Exception, message.Message);
                break;
            case LogSeverity.Verbose:
                logger.LogDebug(message.Exception, message.Message);
                break;
            case LogSeverity.Debug:
                logger.LogDebug(message.Exception, message.Message);
                break;
            default:
                logger.LogError(message.Exception, message.Message);
                break;
        }

        return Task.CompletedTask;
    }
}