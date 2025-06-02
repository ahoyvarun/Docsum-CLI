using System;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;

public static class Translator
{
    public static async Task<string> Translate(string text, string targetLang)
    {
        Console.WriteLine($"\n🌍 Translating summary to '{targetLang}'...");

        string prompt = $"Translate the following text to {targetLang}:\n\n{text}";

        var client = new RestClient("https://api.openai.com/v1/chat/completions");
        var request = new RestRequest()
            .AddHeader("Authorization", $"Bearer {Environment.GetEnvironmentVariable("OPENAI_API_KEY")}")
            .AddHeader("Content-Type", "application/json")
            .AddJsonBody(new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0.3
            });

        var response = await client.PostAsync(request);

        if (!response.IsSuccessful)
        {
            Console.WriteLine("❌ Translation failed: " + response.ErrorMessage);
            return text; // fallback to original
        }

        var parsed = JObject.Parse(response.Content);
        string translated = parsed["choices"]?[0]?["message"]?["content"]?.ToString();

        return translated ?? text;
    }
}