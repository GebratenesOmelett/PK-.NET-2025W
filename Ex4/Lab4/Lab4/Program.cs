using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;

class Program
{
    static async Task Main()
    {
        var urls = new[]
        {
            "https://www.gutenberg.org/files/84/84-0.txt",   
            "https://www.gutenberg.org/files/11/11-0.txt",    
            "https://www.gutenberg.org/files/1661/1661-0.txt",
            "https://www.gutenberg.org/files/2701/2701-0.txt" 
        };

        using HttpClient client = new HttpClient();
        var downloadWatch = Stopwatch.StartNew();

        var downloadTasks = urls.Select(url => client.GetStringAsync(url));
        string[] texts = await Task.WhenAll(downloadTasks);

        downloadWatch.Stop();

        var processWatch = Stopwatch.StartNew();

        var wordCounts = new ConcurrentDictionary<string, int>();

        Parallel.ForEach(texts, text =>
        {
            string cleaned = RemoveGutenbergHeaderAndFooter(text);

            var words = Regex.Split(cleaned, @"\W+")
                             .Where(w => !string.IsNullOrWhiteSpace(w))
                             .Select(w => w.ToLowerInvariant());

            foreach (var word in words)
            {
                wordCounts.AddOrUpdate(word, 1, (_, count) => count + 1);
            }
        });

        processWatch.Stop();

        var top10 = wordCounts
            .OrderByDescending(kv => kv.Value)
            .Take(10);

        Console.WriteLine("Najczęstsze słowa:");
        int rank = 1;
        foreach (var (word, count) in top10)
        {
            Console.WriteLine($"{rank++}. {word}: {count}");
        }

        Console.WriteLine($"\nCzas pobierania: {downloadWatch.Elapsed.TotalSeconds:F2} sekundy");
        Console.WriteLine($"Czas przetwarzania: {processWatch.Elapsed.TotalSeconds:F2} sekundy");
    }

    static string RemoveGutenbergHeaderAndFooter(string text)
    {
        int start = text.IndexOf("*** START");
        int end = text.IndexOf("*** END");

        if (start >= 0 && end > start)
        {
            return text.Substring(start, end - start);
        }

        return text;
    }
}
