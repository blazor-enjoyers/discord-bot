using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Infrastructure.Hosting;

public abstract class BackgroundServiceWithLogging : BackgroundService
{
    private readonly ILogger<BackgroundService> logger;

    protected BackgroundServiceWithLogging(ILogger<BackgroundServiceWithLogging> logger)
    {
        this.logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await base.StartAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "BackgroundService exception");
            throw;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            await base.StopAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "BackgroundService exception");
            throw;
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await ExecuteServiceAsync(stoppingToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "BackgroundService exception");
        }
    }

    protected abstract Task ExecuteServiceAsync(CancellationToken stoppingToken);
}