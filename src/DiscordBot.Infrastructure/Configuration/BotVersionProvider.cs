using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace DiscordBot.Infrastructure.Configuration;

public class BotVersionProvider : IBotVersionProvider
{
    public BotVersionProvider(IHostEnvironment hostEnvironment)
    {
        var assembly = Assembly.GetEntryAssembly();

        if (assembly is not null)
            BotVersion = GetBotVersion(assembly, hostEnvironment);
    }

    public string BotVersion { get; } = "1.0";

    private string GetBotVersion(Assembly assembly, IHostEnvironment environment)
    {
        if (environment.IsProduction())
        {
            var version = assembly.GetName().Version;
            if (version is not null)
                return version.ToString();
        }

        var fileAttribute = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
        if (fileAttribute is null)
            return BotVersion;

        var fileVersion = fileAttribute.Version;
        return environment.IsDevelopment() ? $"{fileVersion}-local" : fileVersion;
    }
}

public interface IBotVersionProvider
{
    string BotVersion { get; }
}