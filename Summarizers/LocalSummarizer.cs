using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Summarizers
public static class LocalSummarizer
{
    private static readonly HashSet<string> stopWords = new()
    {
        "the", "and", "is", "in", "at", "of", "a", "to", "for", "on", "with", "as", "by", "an", "this", "that"
    };

    public static string ExtractKeywordsAndSentences(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "No content to summarize.";

        // Normalize and tokenize words
        string[] words = Regex.Split(text.ToLower(), @"\W+");
        var wordFreq = new Dictionary<string, int>();

        foreach (var word in words)
        {
            if (word.Length < 3 || stopWords.Contains(word)) continue;
            if (!wordFreq.ContainsKey(word)) wordFreq[word] = 0;
            wordFreq[word]++;
        }

        // Get top 5 keywords
        var keywords = wordFreq.OrderByDescending(kvp => kvp.Value)
                               .Take(5)
                               .Select(kvp => kvp.Key)
                               .ToList();

        // Split sentences and select top 2 containing keywords
        var sentences = Regex.Split(text, @"(?<=[.!?])\s+");
        var topSentences = sentences
            .OrderByDescending(s => keywords.Count(k => s.ToLower().Contains(k)))
            .Take(2)
            .ToList();

        return $"=== Local Summary ===\n{string.Join("\n", topSentences)}\n\n🔑 Keywords: {string.Join(", ", keywords)}";
    }
}