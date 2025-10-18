using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

using TextAnalytics.Core;
using TextAnalytics.Services;

namespace TextAnalytics.Tests;

[TestFixture]
public class TextAnalyzerTests
{
    private TextAnalyzer _analyzer;

    [SetUp]
    public void SetUp()
    {
        _analyzer = new TextAnalyzer();
    }

    [Test]
    public void CountWords_Returns2_ForHelloWorld()
    {
        var result = _analyzer.countWords("Hello world!");
        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void CountWords_Returns0_ForEmptyString()
    {
        var result = _analyzer.countWords("");
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void CountWords_Returns0_ForOnlyWhitespace()
    {
        var result = _analyzer.countWords("   \t\n\r   ");
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void CountWords_HandlesMultipleSpacesCorrectly()
    {
        var result = _analyzer.countWords("Hello    world   test");
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void CountCharacters_WithSpaces_ReturnsCorrectCount()
    {
        var result = _analyzer.CountCharacters("Hello world", true);
        Assert.That(result, Is.EqualTo(11));
    }

    [Test]
    public void CountCharacters_WithoutSpaces_ReturnsCorrectCount()
    {
        var result = _analyzer.CountCharacters("Hello world", false);
        Assert.That(result, Is.EqualTo(10));
    }

    [Test]
    public void FindMostCommonWord_ReturnsFirstWord_WhenTie()
    {
        var result = _analyzer.getMostCommonWord("apple banana apple banana");
        Assert.That(result, Is.EqualTo("apple"));
    }

    [Test]
    public void FindMostCommonWord_ReturnsMostFrequentWord()
    {
        var result = _analyzer.getMostCommonWord("cat dog cat bird cat dog");
        Assert.That(result, Is.EqualTo("cat"));
    }

    [Test]
    public void CountSentences_ReturnsCorrectCount_WithMultipleDelimiters()
    {
        var result = _analyzer.countSentences("Hello world! How are you? I'm fine.");
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void Analyze_CompleteAnalysis_ReturnsCorrectStatistics()
    {
        var text = "Hello world! This is a test. Another sentence.";
        var result = _analyzer.Analyze(text);

        Assert.That(result.WordCount, Is.EqualTo(8));
        Assert.That(result.SentenceCount, Is.EqualTo(3));
        Assert.That(result.CharactersWithSpaces, Is.GreaterThan(0));
        Assert.That(result.Letters, Is.GreaterThan(0));
    }

    [Test]
    public void CalculateAverageWordLength_ReturnsCorrectValue()
    {
        var result = _analyzer.countAverageWordsLength("hi hello");
        Assert.That(result, Is.EqualTo(3.5).Within(0.01));
    }

    [Test]
    public void FindLongestAndShortestWord_ReturnsCorrectWords()
    {
        var text = "a bb ccc longest shortest";
        var analyzer = new TextAnalyzer();
        
        var stats = analyzer.Analyze(text);
        
        Assert.That(stats.LongestWord, Is.EqualTo("shortest"));
        Assert.That(stats.ShortestWord, Is.EqualTo("a"));
    }

    [Test]
    public void CountUniqueWords_IgnoresCase()
    {
        var result = _analyzer.countUniqueWords("Hello hello HELLO");
        Assert.That(result, Is.EqualTo(1));
    }
}

[TestFixture]
public class ServiceTests
{
    [Test]
    public void DependencyInjection_ResolvesServicesCorrectly()
    {
        var services = new ServiceCollection()
            .AddSingleton<ILoggerService, ConsoleLogger>()
            .AddSingleton<IInputProvider, ConsoleInputProvider>()
            .AddSingleton<TextAnalyzer>()
            .BuildServiceProvider();

        var analyzer = services.GetRequiredService<TextAnalyzer>();
        var logger = services.GetRequiredService<ILoggerService>();
        var inputProvider = services.GetRequiredService<IInputProvider>();

        Assert.Multiple(() =>
        {
            Assert.That(analyzer, Is.Not.Null);
            Assert.That(logger, Is.Not.Null);
            Assert.That(inputProvider, Is.Not.Null);
        });
    }
}