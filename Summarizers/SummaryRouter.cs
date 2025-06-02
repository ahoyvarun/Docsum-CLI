using System;
using System.Threading.Tasks;

namespace Summarizers
public static class SummaryRouter
{
    public static async Task<string> GetSmartSummary(string inputText)
    {
        string classification = await DocumentClassifier.Classify(inputText);

        Console.WriteLine($"📄 Detected Document Type: {classification}");

        if (classification == "contract" || classification == "report")
        {
            Console.WriteLine("🤖 Using OpenAI for deep summarization...");
            return await OpenAIClient.GetSummary(inputText);
        }
        else
        {
            Console.WriteLine("🧠 Using Local AI summarizer...");
            return LocalSummarizer.ExtractKeywordsAndSentences(inputText); // You'll define this next
        }
    }
}