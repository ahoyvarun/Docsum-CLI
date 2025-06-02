using NUnit.Framework;

namespace tests
[TestFixture]
public class SummarizerTests
{
    [Test]
    public void LocalSummarizer_Should_SummarizeBasicInput()
    {
        string input = "Artificial intelligence is transforming industries. Machine learning enables systems to learn from data.";
        string summary = LocalSummarizer.ExtractKeywordsAndSentences(input);

        Assert.That(summary, Is.Not.Null);
        Assert.That(summary.ToLower(), Does.Contain("intelligence").Or.Contain("learning"));
    }

    [Test]
    public void SummaryRouter_Should_UseLocalSummarizer_WhenTextIsShort()
    {
        string shortText = "Meeting notes from 12th Jan. Project deadline is near.";
        string summary = SummaryRouter.GetSmartSummary(shortText).Result;

        Assert.That(summary, Is.Not.Null);
        Assert.That(summary.Length, Is.GreaterThan(0));
    }
}