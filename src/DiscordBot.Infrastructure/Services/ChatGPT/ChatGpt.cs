using System.Text;
using DiscordBot.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DiscordBot.Infrastructure.Services.ChatGPT;

public class ChatGpt : IChatGpt
{
    private readonly IChatGptOptions _options;
    private readonly ILogger<ChatGpt> _logger;

    public ChatGpt(IChatGptOptions options, ILogger<ChatGpt> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task<string> AskQuestion(string question)
    {
        if (string.IsNullOrWhiteSpace(question)) return "There is not question!";
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("authorization", $"Bearer {_options.BearerToken}");
        var content = new StringContent("{\"model\": \"text-davinci-001\", \"prompt\": \""+ question +"\",\"temperature\": 1,\"max_tokens\": 100}",
            Encoding.UTF8, "application/json");
        
        var response = await httpClient.PostAsync("https://api.openai.com/v1/completions", content);
        var responseString = await response.Content.ReadAsStringAsync();

        try
        {
            var dynamicData = JsonConvert.DeserializeObject<dynamic>(responseString);
            string choiceText = dynamicData!.choices[0].text;
            return choiceText;
        }
        catch (Exception ex)    
        {
            _logger.LogError(ex, "{Type} error. Deserialization error", typeof(ChatGpt));
        }

        return "There is not question!";
    }
}