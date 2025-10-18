namespace TextAnalytics.Core;

using System.Globalization;
using System.Text.RegularExpressions;
public sealed class TextAnalyzer
{
    public TextStatistics Analyze(string text)
    {
        int charsWithSpaces = CountCharacters(text, includeSpaces: true);
        int charsWithoutSpaces = CountCharacters(text, includeSpaces: false);
        int letters = text.Count(char.IsLetter);
        int digits = text.Count(char.IsDigit);
        int punctuations = text.Count(c => char.IsPunctuation(c));

        int wordCount = countWords(text);
        int uniqueWordCount = countUniqueWords(text);
        string mostCommonWord = getMostCommonWord(text);
        double averageWordLength = countAverageWordsLength(text);
        string longestWord = getLongestWord(text);
        string shortestWord = getShortestWord(text);

        int sentenceCount = countSentences(text);
        double averageWordsPerSentence = countAverageWordsPerSentence(text);
        string longestSentence = getLongestSentence(text);
        
        return new TextStatistics(charsWithSpaces, charsWithoutSpaces, letters, digits, punctuations, wordCount, uniqueWordCount
        ,mostCommonWord, averageWordLength, longestWord, shortestWord, sentenceCount, averageWordsPerSentence, longestSentence);

    }

    public int CountCharacters(string text, bool includeSpaces = true)
    {
        if (includeSpaces) return text.Length;
        return text.Count(c => !char.IsWhiteSpace(c));
    }

    public int countWords(string text)
    {
        if (string.IsNullOrEmpty(text)) return 0;
        string[] words = text.Split(new char[]{
            ' ', '\t', '\n', '\r'
        }, StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }
    public int countUniqueWords(string text)
    {
        if (string.IsNullOrEmpty(text)) return 0;
        string[] words = text.ToLower().Split(new char[]{
            ' ', '\t', '\n', '\r', '.', ',', '!', '?', ';',':'
        }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
        return words.Length;
    }

    public string getMostCommonWord(string text)
    {
        string[] words = text.Split(new char[]{
            ' ', '\t', '\n', '\r'
        }, StringSplitOptions.RemoveEmptyEntries);
        var commonWords = words.ToList().GroupBy(e => e).Select(g => new {Value = g.Key, Count = g.Count()}).OrderByDescending(e => e.Count).Take(1);
        return commonWords.First().Value;
    }
    public double countAverageWordsLength(string text)
    {
        if (string.IsNullOrEmpty(text)) return 0;
        string[] words = text.Split(new char[]{
            ' ', '\t', '\n', '\r'
        }, StringSplitOptions.RemoveEmptyEntries);

        double average = words.Average(w => w.Length);
        return average;
    }

    public string getLongestWord(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        string[] words = text.Split(new char[]{
            ' ', '\t', '\n', '\r'
        }, StringSplitOptions.RemoveEmptyEntries);
        return words.OrderByDescending(w => w.Length).First();
    }

    public string getShortestWord(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        string[] words = text.Split(new char[]{
            ' ', '\t', '\n', '\r'
        }, StringSplitOptions.RemoveEmptyEntries);
        return words.OrderByDescending(w => w.Length).Last();
    }

    public int countSentences(string text)
    {
        if (string.IsNullOrEmpty(text)) return 0;
        string[] sentences = text.Split(new char[]{
            '.','?','!'
        }, StringSplitOptions.RemoveEmptyEntries);
        
        int count = 0;
        foreach (string sentence in sentences)
        {
            if (!string.IsNullOrEmpty(sentence))
                count++;
        }

        return count;
    }

    public double countAverageWordsPerSentence(string text)
    {
        if (string.IsNullOrEmpty(text)) return 0;

        string[] sentences = text.Split(new char[]
        {
            '.', '?', '!'
        }, StringSplitOptions.RemoveEmptyEntries).Where(s => !string.IsNullOrEmpty(s)).ToArray();

        if (sentences.Length == 0) return 0;

        double totalWords = 0;
        foreach (string sentence in sentences)
        {
            string[] words = sentence.Split(new char[]
            {
                ' ', '\t', '\n', '\r', '.', ',', '!', '?', ';', ':'
            }, StringSplitOptions.RemoveEmptyEntries);
            totalWords += words.Length;
        }

        return totalWords / sentences.Length;
    }

    public string getLongestSentence(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        string[] sentences = text.Split(new char[]
        {
            '.', '?', '!'
        }, StringSplitOptions.RemoveEmptyEntries).Where(s => !string.IsNullOrEmpty(s)).ToArray();
        if (sentences.Length == 0) return string.Empty;

        string longest = sentences.OrderByDescending(s => s.Split(new char[]
        {
            ' ', '\t', '\n', '\r', '.', ',', '!', '?', ';', ':'
        }, StringSplitOptions.RemoveEmptyEntries).Length).First();

        return longest;
    }
}
public sealed record TextStatistics(
    int CharactersWithSpaces,
    int CharactersWithoutSpaces,
    int Letters,
    int Digits,
    int Punctuation,
    int WordCount,
    int UniqueWordCount,
    string MostCommonWord,
    double AverageWordLength,
    string LongestWord,
    string ShortestWord,
    int SentenceCount,
    double AverageWordsPerSentence,
    string LongestSentence
);