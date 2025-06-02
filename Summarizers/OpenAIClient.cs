using RestSharp;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System;

namespace Summarizers
public static class OpenAIClient
{
    private static readonly string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

    public static async Task<string> GetSummary(string inputText)
    {
        var client = new RestClient("https://api.openai.com/v1/chat/completions");
        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Authorization", $"Bearer {apiKey}");
        request.AddHeader("Content-Type", "application/json");

        request.AddJsonBody(new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "Summarize the following legal/business document in bullet points and extract named entities." },
                new { role = "user", content = inputText }
            }
        });

        var response = await client.ExecuteAsync(request);
        if (response.Content == null)
        {
            return "❌ No response from OpenAI.";
        }
        Console.WriteLine("\n🔍 Raw Response from OpenAI:\n");
        Console.WriteLine(response.Content);
        var json = JObject.Parse(response.Content);
        return json["choices"]?[0]?["message"]?["content"]?.ToString() ?? "❌ Failed to parse response.";
    }
}