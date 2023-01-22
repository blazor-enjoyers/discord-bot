using Discord.Commands;
using Discord.Interactions;
using DiscordBot.Infrastructure.Services.ChatGPT;

namespace DiscordBot.Infrastructure.Commands.Modules;

public class ChatGptModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IChatGpt _chatGpt;
    
    public ChatGptModule(IChatGpt chatGpt)
    {
        _chatGpt = chatGpt;
    }
    
    [SlashCommand("ask", "Ask ChatGPT a question")]
    public async Task AskGptAsync([Remainder] string question)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            await RespondAsync("Can't work with that! >:(");
            return;
        }
        var response = await _chatGpt.AskQuestion(question);
        await RespondAsync(response);
    }
}