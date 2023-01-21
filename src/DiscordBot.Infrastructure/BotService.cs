using Discord;
using Discord.WebSocket;
using DiscordBot.Infrastructure.Commands;
using DiscordBot.Infrastructure.Configuration;
using DiscordBot.Infrastructure.Hosting;
using DiscordBot.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Infrastructure;

public class BotService : BackgroundServiceWithLogging
{
    private readonly CommandHandler commandHandler;
    private readonly DiscordSocketClient discordClient;
    private readonly LoggingHandler loggingHandler;
    private readonly IBotOptions options;
    private readonly IBotVersionProvider versionProvider;

    public BotService(DiscordSocketClient discordClient,
        IBotVersionProvider versionProvider,
        LoggingHandler loggingHandler,
        IBotOptions options,
        ILogger<BotService> logger, CommandHandler commandHandler)
        : base(logger)
    {
        this.discordClient = discordClient;
        this.versionProvider = versionProvider;
        this.loggingHandler = loggingHandler;
        this.options = options;
        this.commandHandler = commandHandler;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await commandHandler.InitializeAsync();
        discordClient.GuildAvailable += OnGuildAvailable;
        loggingHandler.Initialize();
        await discordClient.LoginAsync(TokenType.Bot, options.Token);
        await base.StartAsync(cancellationToken);
    }

    private async Task OnGuildAvailable(SocketGuild guild)
        => await guild.CurrentUser.ModifyAsync(props => props.Nickname = options.Nickname);

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await discordClient.StopAsync();
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteServiceAsync(CancellationToken stoppingToken)
    {
        await discordClient.StartAsync();
        await discordClient.SetGameAsync($"v{versionProvider.BotVersion}");
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}