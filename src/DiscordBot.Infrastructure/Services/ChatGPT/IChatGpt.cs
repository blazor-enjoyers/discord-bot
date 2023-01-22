namespace DiscordBot.Infrastructure.Services.ChatGPT;

public interface IChatGpt
{ 
    Task<string> AskQuestion(string question);
}